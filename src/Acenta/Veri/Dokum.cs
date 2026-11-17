using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Acenta.Veri;

/// <summary>Bir sütunun başlığı, genişliği (PDF için göreli) ve para alanı olup olmadığı.</summary>
public record Sutun(string Baslik, float Genislik = 1, bool Para = false, bool Bos = false);

/// <summary>
/// Paneldeki tabloları Excel (.xlsx) ve PDF olarak üretir: yolcu listesi ve kalkış raporu.
/// Hücre değerleri string, int ya da decimal olabilir; Excel'de sayılar sayı olarak yazılır.
/// </summary>
public static class Dokum
{
    public static byte[] Excel(string sayfaAdi, string baslik, string altBaslik, Sutun[] sutunlar, IEnumerable<object?[]> satirlar, object?[]? toplam = null)
    {
        using var kitap = new XLWorkbook();
        var ws = kitap.Worksheets.Add(sayfaAdi.Length > 31 ? sayfaAdi[..31] : sayfaAdi);
        ws.Cell(1, 1).Value = baslik;
        ws.Cell(1, 1).Style.Font.SetBold().Font.SetFontSize(14);
        ws.Cell(2, 1).Value = altBaslik;
        ws.Cell(2, 1).Style.Font.SetFontColor(XLColor.Gray);

        var r = 4;
        for (var c = 0; c < sutunlar.Length; c++)
        {
            var h = ws.Cell(r, c + 1);
            h.Value = sutunlar[c].Baslik;
            h.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#F3E9DA")).Border.SetBottomBorder(XLBorderStyleValues.Thin);
        }
        var ilk = r + 1;
        foreach (var satir in satirlar)
        {
            r++;
            for (var c = 0; c < sutunlar.Length; c++) Yaz(ws.Cell(r, c + 1), satir.ElementAtOrDefault(c), sutunlar[c]);
        }
        if (toplam is not null)
        {
            r++;
            for (var c = 0; c < sutunlar.Length; c++) Yaz(ws.Cell(r, c + 1), toplam.ElementAtOrDefault(c), sutunlar[c]);
            ws.Row(r).Style.Font.SetBold().Border.SetTopBorder(XLBorderStyleValues.Thin);
        }
        if (r >= ilk) ws.Range(4, 1, r, sutunlar.Length).SetAutoFilter();
        ws.SheetView.FreezeRows(4);
        ws.Columns(1, sutunlar.Length).AdjustToContents(4, r);
        for (var c = 0; c < sutunlar.Length; c++)
            if (sutunlar[c].Bos) ws.Column(c + 1).Width = 18;

        using var akis = new MemoryStream();
        kitap.SaveAs(akis);
        return akis.ToArray();
    }

    private static void Yaz(IXLCell hucre, object? deger, Sutun sutun)
    {
        switch (deger)
        {
            case null: break;
            case decimal d:
                hucre.Value = d;
                hucre.Style.NumberFormat.Format = sutun.Para ? "#,##0 \"₺\"" : "#,##0";
                break;
            case int i: hucre.Value = i; break;
            case DateTime t:
                hucre.Value = t;
                hucre.Style.DateFormat.Format = "dd.mm.yyyy";
                break;
            default: hucre.Value = deger.ToString(); break;
        }
    }

    public static byte[] Pdf(string baslik, string altBaslik, Sutun[] sutunlar, IEnumerable<object?[]> satirlar, object?[]? toplam = null, bool yatay = true)
    {
        var liste = satirlar.ToList();
        return Document.Create(belge => belge.Page(sayfa =>
        {
            sayfa.Size(yatay ? PageSizes.A4.Landscape() : PageSizes.A4);
            sayfa.Margin(24);
            sayfa.DefaultTextStyle(x => x.FontSize(9));

            sayfa.Header().PaddingBottom(10).Row(satir =>
            {
                satir.RelativeItem().Column(k =>
                {
                    k.Item().Text(baslik).FontSize(15).Bold().FontColor("#2a1d2c");
                    k.Item().Text(altBaslik).FontColor(Colors.Grey.Darken1);
                });
                satir.ConstantItem(220).AlignRight().Column(k =>
                {
                    k.Item().AlignRight().Text(Ayarlar.SiteAyarlari.SiteAdi).Bold().FontColor("#b4532a");
                    k.Item().AlignRight().Text($"TÜRSAB belge no {Ayarlar.SiteAyarlari.TursabBelgeNo}").FontSize(8).FontColor(Colors.Grey.Darken1);
                });
            });

            sayfa.Content().Table(tablo =>
            {
                tablo.ColumnsDefinition(c => { foreach (var s in sutunlar) c.RelativeColumn(s.Genislik); });
                tablo.Header(h =>
                {
                    foreach (var s in sutunlar)
                        h.Cell().Background("#F3E9DA").BorderBottom(1).BorderColor("#c9b9a4").Padding(4)
                            .Text(s.Baslik).Bold();
                });
                var sira = 0;
                foreach (var satir in liste)
                {
                    string zemin = sira++ % 2 == 0 ? "#FFFFFF" : "#FBF7F1";
                    for (var c = 0; c < sutunlar.Length; c++)
                        Hucre(tablo.Cell().Background(zemin).BorderBottom(0.5f).BorderColor("#e8ddcd").Padding(4).MinHeight(sutunlar[c].Bos ? 18 : 0), satir.ElementAtOrDefault(c), sutunlar[c]);
                }
                if (toplam is not null)
                    for (var c = 0; c < sutunlar.Length; c++)
                        Hucre(tablo.Cell().BorderTop(1).BorderColor("#2a1d2c").Padding(4), toplam.ElementAtOrDefault(c), sutunlar[c], kalin: true);
            });

            sayfa.Footer().Row(satir =>
            {
                satir.RelativeItem().Text($"{Ayarlar.SiteAyarlari.FirmaUnvani} · {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(7.5f).FontColor(Colors.Grey.Darken1);
                satir.ConstantItem(80).AlignRight().Text(m =>
                {
                    m.DefaultTextStyle(x => x.FontSize(7.5f).FontColor(Colors.Grey.Darken1));
                    m.CurrentPageNumber(); m.Span(" / "); m.TotalPages();
                });
            });
        })).GeneratePdf();
    }

    private static void Hucre(IContainer kap, object? deger, Sutun sutun, bool kalin = false)
    {
        var metin = deger switch
        {
            null => "",
            decimal d => sutun.Para ? TurServisi.Para(d) : d.ToString("#,0", TurServisi.Tr),
            DateTime t => t.ToString("dd.MM.yyyy"),
            _ => deger.ToString() ?? "",
        };
        var hizali = deger is decimal or int ? kap.AlignRight() : kap;
        var t2 = hizali.Text(metin);
        if (kalin) t2.Bold();
    }
}
