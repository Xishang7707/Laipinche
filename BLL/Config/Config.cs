﻿namespace Laipinche.BLL.Config
{
    public class Config
    {
        /// <summary>
        /// 请求过期时间30秒
        /// </summary>
        public const long Timeout = 30000;
        public static string Pubkey = @"<RSAKeyValue><Modulus>rtRyIaULclRYbqCI7FHqBAGeP4tIkgK7jJg5VMwtQrkqS8aE/0A5CiWwKne5uCfy9nyBYeRTXT3vLdXqcafWll41on7WuaAQZCMNZOITyXYWmeVtEo0B+kyF/BDMX2LgtV/o1Fs9SqJNL05rALAheS52O/qMa//iLaxT0D2+cN0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        //public static string Prvkey = @"<RSAKeyValue><Modulus>rtRyIaULclRYbqCI7FHqBAGeP4tIkgK7jJg5VMwtQrkqS8aE/0A5CiWwKne5uCfy9nyBYeRTXT3vLdXqcafWll41on7WuaAQZCMNZOITyXYWmeVtEo0B+kyF/BDMX2LgtV/o1Fs9SqJNL05rALAheS52O/qMa//iLaxT0D2+cN0=</Modulus><Exponent>AQAB</Exponent><P>xnBkwM0PgyPsTlHiZJBltpM1eEftTraxFHlN15jbr8UlBrR15s8t/CGbMFj6rg9BFNhjfQSrZSYfU90dJsDt1w==</P><Q>4YrkVNupc1zwIPdjBlBIrUjtl3LJE9zjquCYai5IPaxxCBcQeqa1pwTtYSNn8FVk5ASuve/x3iiTBLLBtww4aw==</Q><DP>sbv7UgCY80Gapq+K0b3PjE6fpqLttwHE62yWvxMnOXCfcDj0HQ3Vxy0mfZBEvt/kz9Hckh47kXYquweh05Mbsw==</DP><DQ>qN2AzzgXytQOxd/zOErr3zH3EalTe5eqrcmy0+0LEYIGrvP11p7y6rX5tPz5WSvWehI3cuYvxRHD4YSlHRP9OQ==</DQ><InverseQ>G/Z/FO+zhDuZkEG/2AIsQBR1CpQLY9ESuqZBFDJzdmCYNmcvTDcV/UYZEEYepIrK5gFkETgA8CpjGF3OCSYqBg==</InverseQ><D>cuzSjc1ZxfEuJKLA/2AljjamGS1tKhZDxXs3jMefJRJlfGRmfwhvtnzp3jDP0//m1iMoRfp61ntlI+NP9yCWjTFxD991TxgnctQ4I1sPLEeCSSXW1oFKCtWPWlK5JMngn1yJ//0nGEy30rm1vhD46Hp+UZRRraQG6vY4gw5Hpn0=</D></RSAKeyValue>";
        public static string Prvkey = @"<RSAKeyValue><Modulus>3C3llUq20aWBzBHAib7wIhGV7Pk+7vNOE4Ci2TinoDBA3xcch9nEB52IlghdUTLdk+Z5Wcj5SDjQkPVFMLvUBVGzM9Rv21cckvDcsUNqhDCQdpPxwNsFAeiQcMOyjoo6Q9ZzH4Qd+NbtWMV7kY3lES/AvzoKxGueMiZXa+iSCq50CQ1l7kmcuWaCWygOd7UcqLidg2oCdh1cN547Rv4RJONh771RpdwbnC6OSniLLkXvk5NDWk2r+v1Ngyh72LGuMiiySdhiuEhnMik797eF3cTm6IHKEaXAeYJdgt9G95Xjrj4t7WYxT1jpTYkK7c73RGyAvaDuH8GDlGoKqFJyqw==</Modulus><Exponent>AQAB</Exponent><P>/xyowpXrY/++z4iqaGja3FYJcJTvJwxsdDZAWzJlgGltRQ8otbzbPQZyNILitFprq+zjr6IKEgYVsZvQgKu09M7OAQ3kuzbob8iT+w3mtGm49iOcQEBaJTkRexY/2r8A3xoq4oovGoXN6uwgE2+k4KY/Ai5HLUWQ1ia9vWbY+K0=</P><Q>3PIbjwMizZ2HLZHJ5mVReI1Lb8uP+p3y5+rusTYtYtmiTPGKFelBp2Nfe8sgN/Ub9JmdBlUzBEunY8s3B1oWF3gqms5+YZIjTWDafEXncfuR1HBdjv4aJbfma+t3PPUqL9LDzowm7HKJsApJclqQH3IPrHH03Fe7hTKbaPGWS7c=</Q><DP>b9TnYc2Ij6CXy7A0S2rTrk0E/FY9dBetH1B8671u41jT7rcTeH+ed+TwgXZy3O7aTladBOv6kCYcsmwEfbNHsx1JDwtBHpSWVhJOVHqmjKXfz/6SS1QqcDDRKez59vFN938oF4DN30mSmA4EjaCeXWBjNJiG2SCqukXnXTTNnk0=</DP><DQ>DODf0lowcSO5eWKCDjfho9E4kG+hGM6UFgiPmSy7QIPG/g0mK1jJRuR38FNKsXhMBNnfWTsmGh3+x8ZUks0hdITAlv1qSG2oOR5oHc5FS1OKkINsMKWHCKPLfobfEGKJ7veuCiAJNMcIV4aB+XRl6i9P0pFh4GYMvBrTDC7ov8k=</DQ><InverseQ>O5pEvn0QHPaQIscBzpkgA8lHATZAhmS5Tn3C4DGTAyK8B0eVicwfZU6tEGJ3C94Me/Jmt5f5nZZZV5f8eWtLMRokGDpi8xl0WmhUDqoQ4Zm8qYorgzhswH3HNn4F1JRfDSrJtENmJoia7Srx9Q9RkpIPq2YjdqQ6df63aWurA1k=</InverseQ><D>Y8OMFjQvWBjVEiEtC3zvk5BCTYi2AlFOHDoaD7DDCysyCaOQMuHyF71CiTCrD8KrcXtpmswcX6QWpMftnOJOdEaLmz1znYAAG3h1lwBEADd9Da2FSewzfaTh5aZWfo2VTu2ZZjtX1UuVaba5gb84fqwTetIS1OMqX/DNhcBwg9OmXSrFwsTUiH8XwbRwcKHgWskRhwPD7a1ensusAe2wcXWm7tnm03XRzckUcHLfFHBIo1S3qZ53IaROiDhKIlKMJWO5lckgcgEbzi7KMLbqok06Dvc6KH1tkvuiLG2H1NVQJ3MbztqtRJT76TAIBJSb0WqFrPntLEtnejPyL1xxuQ==</D></RSAKeyValue>";
    }
}