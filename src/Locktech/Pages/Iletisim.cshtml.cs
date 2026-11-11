using Locktech.Ayarlar;
using Locktech.Teklif;

namespace Locktech.Pages;

public class IletisimModel(PostaServisi posta, ILogger<IletisimModel> log) : TeklifSayfasi(posta, log);
