// Decompiled with JetBrains decompiler
// Type: GHIElectronics.TinyCLR.Drivers.Azure.SAS.SharedAccessSignatureConstants
// Assembly: GHIElectronics.TinyCLR.Drivers.Azure.SAS, Version=2.2.0.4000, Culture=neutral, PublicKeyToken=null
// MVID: D8863A6C-0A2F-4AB9-9C08-3E2B33F9F218
// Assembly location: D:\experiment\TinyCLR2\SAS\SASApp\SASApp\bin\Debug\GHIElectronics.TinyCLR.Drivers.Azure.SAS.dll

using System;

namespace SASBuilder
{
  internal static class SharedAccessSignatureConstants
  {
    public const int MaxKeyNameLength = 256;
    public const int MaxKeyLength = 256;
    public const string SharedAccessSignature = "SharedAccessSignature";
    public const string AudienceFieldName = "sr";
    public const string SignatureFieldName = "sig";
    public const string KeyNameFieldName = "skn";
    public const string ExpiryFieldName = "se";
    public const string SignedResourceFullFieldName = "SharedAccessSignature sr";
    public const string KeyValueSeparator = "=";
    public const string PairSeparator = "&";
    public static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    public static readonly TimeSpan MaxClockSkew = new TimeSpan(0, 5, 0);
  }
}
