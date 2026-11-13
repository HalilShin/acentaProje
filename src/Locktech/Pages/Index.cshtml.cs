using Locktech.Ayarlar;
using Locktech.Teklif;

namespace Locktech.Pages;

public class IndexModel(PostaServisi posta, ILogger<IndexModel> log) : TeklifSayfasi(posta, log);
