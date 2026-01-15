# PLCSIM Kurulum Rehberi

Bu doküman, IndustrialMonitor uygulamasını **PLCSIM Advanced** ile test etmek için gereken adımları içerir.

## 1. TIA Portal'da Proje Oluşturma

1. TIA Portal'ı açın
2. Yeni proje oluşturun
3. **S7-1500** CPU ekleyin (örn: CPU 1516-3 PN/DP)

## 2. Data Block (DB1) Oluşturma

TIA Portal'da aşağıdaki yapıda bir **Global Data Block** oluşturun:

| Name | Data Type | Offset | Açıklama |
|------|-----------|--------|----------|
| TankLevel | Real | 0.0 | Tank seviyesi (%) |
| Temperature | Real | 4.0 | Sıcaklık (°C) |
| Pressure | Real | 8.0 | Basınç (bar) |
| MotorSpeed | DInt | 12.0 | Motor hızı (rpm) |
| SystemRunning | Bool | 16.0 | Sistem çalışıyor mu? |

### TIA Portal'da Adımlar:
1. **Program blocks** → **Add new block** → **Data block (DB)**
2. İsim: `DB1` veya `ProcessData`
3. Yukarıdaki tabloyu yapıya ekleyin
4. **Compile** edin

## 3. PLCSIM Advanced Ayarları

1. PLCSIM Advanced'ı başlatın
2. **New Instance** oluşturun
3. CPU tipini seçin (S7-1500)
4. **Network settings**:
   - Virtual Ethernet Adapter kullanın
   - IP: `192.168.0.1` (veya uygulamadaki IP ile eşleştirin)
5. TIA Portal projesini **Download** edin

## 4. IP Adresi Ayarlama

Eğer farklı bir IP kullanıyorsanız, `PlcService.cs` dosyasında güncelleyin:

```csharp
public string IpAddress { get; set; } = "192.168.0.1"; // Buraya PLCSIM IP'sini yazın
```

## 5. Test Etme

1. PLCSIM'in çalıştığından emin olun
2. IndustrialMonitor uygulamasını başlatın
3. Alt kısımdaki **"Connect PLC"** butonuna basın
4. Bağlantı başarılıysa "Connected to PLC at..." mesajını göreceksiniz
5. TIA Portal'dan DB1 değerlerini değiştirin ve uygulamada güncellendiğini gözlemleyin

## 6. Bağlantı Sorunları

Bağlantı kurulamıyorsa:
- Windows Firewall'u kontrol edin
- PLCSIM IP adresine ping atın: `ping 192.168.0.1`
- TIA Portal'da **Put/Get** iletişimi aktif edin:
  - CPU Properties → Protection & Security → Enable PUT/GET

## Örnek Test Değerleri

TIA Portal'da DB1'e şu değerleri yazarak test edebilirsiniz:

```
DB1.TankLevel := 75.5;
DB1.Temperature := 52.3;
DB1.Pressure := 3.8;
DB1.MotorSpeed := 1680;
DB1.SystemRunning := TRUE;
```
