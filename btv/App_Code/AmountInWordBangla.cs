using System;

public class AmountInWordBangla
{
    private string Words_1_19(int num)
    {
        switch (num)
        {
            case 1:
                return "GK";

            case 2:
                return "`yB";

            case 3:
                return "wZb";

            case 4:
                return "Pvi";

            case 5:
                return "cuvP";

            case 6:
                return "Qq";

            case 7:
                return "mvZ";

            case 8:
                return "AvU";

            case 9:
                return "bq";

            case 10:
                return "`k";

            case 11:
                return "GMvi";

            case 12:
                return "ev‡iv";

            case 13:
                return "†Z‡iv";

            case 14:
                return "†PŠ\x00cf";

            case 15:
                return "c‡b‡iv";

            case 0x10:
                return "‡lvj";

            case 0x11:
                return "m‡Zi";

            case 0x12:
                return "AvVvi";

            case 0x13:
                return "Dwbk";
        }
        return null;
    }

    private string Words_1_99(int num)
    {
        string str = null;
        int num2 = 0;
        int num3 = 0;
        num2 = num / 10;
        if (num2 <= 1)
        {
            str = str + " " + Words_1_19(num);
        }
        else
        {
            switch (num2)
            {
                case 2:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKyk";
                                break;

                            case 2:
                                str = "evBk";
                                break;

                            case 3:
                                str = "‡ZBk";
                                break;

                            case 4:
                                str = "Pwe\x00a1k";
                                break;

                            case 5:
                                str = "cwuPk";
                                break;

                            case 6:
                                str = "Qvwe\x00a1k";
                                break;

                            case 7:
                                str = "mvZvBk";
                                break;

                            case 8:
                                str = "AvUvBk";
                                break;

                            case 9:
                                str = "DbwZ\x00aak";
                                break;
                        }
                        break;
                    }
                    str = "wek";
                    break;

                case 3:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKw\x00cek";
                                break;

                            case 2:
                                str = "ew\x00cek";
                                break;

                            case 3:
                                str = "‡Zw\x00cek";
                                break;

                            case 4:
                                str = "‡PŠw\x00cek";
                                break;

                            case 5:
                                str = "cqw\x00cek";
                                break;

                            case 6:
                                str = "Qw\x00cek";
                                break;

                            case 7:
                                str = "mvBw\x00cek";
                                break;

                            case 8:
                                str = "AvUw\x00cek";
                                break;

                            case 9:
                                str = "DbPwj\x00ack";
                                break;
                        }
                        break;
                    }
                    str = "w\x00cek";
                    break;

                case 4:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKPwj\x00ack";
                                break;

                            case 2:
                                str = "weqvwj\x00ack";
                                break;

                            case 3:
                                str = "‡ZZvwj\x00ack";
                                break;

                            case 4:
                                str = "Pyqvwj\x00ack";
                                break;

                            case 5:
                                str = "cqZvwj\x00ack";
                                break;

                            case 6:
                                str = "‡QPwj\x00ack";
                                break;

                            case 7:
                                str = "mvZPwj\x00ack";
                                break;

                            case 8:
                                str = "AvUPwj\x00ack";
                                break;

                            case 9:
                                str = "Dbc\x00c2vk";
                                break;
                        }
                        break;
                    }
                    str = "Pwj\x00adk";
                    break;

                case 5:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKvbœ";
                                break;

                            case 2:
                                str = "evnvbœ";
                                break;

                            case 3:
                                str = "‡Z\x00e0vbœ";
                                break;

                            case 4:
                                str = "Pyqvbœ";
                                break;

                            case 5:
                                str = "c\x00c2vbœ";
                                break;

                            case 6:
                                str = "Qv\x00e0vbœ";
                                break;

                            case 7:
                                str = "mvZvbœ";
                                break;

                            case 8:
                                str = "AvUvbœ";
                                break;

                            case 9:
                                str = "DblvU";
                                break;
                        }
                        break;
                    }
                    str = "c\x00c2vk";
                    break;

                case 6:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKlw\x00c6";
                                break;

                            case 2:
                                str = "evlw\x00c6";
                                break;

                            case 3:
                                str = "‡Zlw\x00c6";
                                break;

                            case 4:
                                str = "‡PŠlw\x00c6";
                                break;

                            case 5:
                                str = "cqlw\x00c6";
                                break;

                            case 6:
                                str = "‡Qlw\x00c6";
                                break;

                            case 7:
                                str = "mvZlw\x00c6";
                                break;

                            case 8:
                                str = "AvUlw\x00c6";
                                break;

                            case 9:
                                str = "Dbm‡\x00cbvi";
                                break;
                        }
                        break;
                    }
                    str = "lvU";
                    break;

                case 7:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKv\x00cbi";
                                break;

                            case 2:
                                str = "evnv\x00cbi";
                                break;

                            case 3:
                                str = "‡Znv\x00cbi";
                                break;

                            case 4:
                                str = "Pyqv\x00cbi";
                                break;

                            case 5:
                                str = "cPuv\x00cbi";
                                break;

                            case 6:
                                str = "‡Qqv\x00cbi";
                                break;

                            case 7:
                                str = "mvZv\x00cbi";
                                break;

                            case 8:
                                str = "AvUv\x00cbi";
                                break;

                            case 9:
                                str = "DbAvwk";
                                break;
                        }
                        break;
                    }
                    str = "m‡\x00cbvi";
                    break;

                case 8:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKvwk";
                                break;

                            case 2:
                                str = "weivwk";
                                break;

                            case 3:
                                str = "‡Zivwk";
                                break;

                            case 4:
                                str = "Pyivwk";
                                break;

                            case 5:
                                str = "cPuvwk";
                                break;

                            case 6:
                                str = "‡Qqvwk";
                                break;

                            case 7:
                                str = "mvZvwk";
                                break;

                            case 8:
                                str = "AvUvwk";
                                break;

                            case 9:
                                str = "Dbbe\x00a1B";
                                break;
                        }
                        break;
                    }
                    str = "Avwk";
                    break;

                case 9:
                    num3 = num % 10;
                    if (num3 != 0)
                    {
                        switch (num3)
                        {
                            case 1:
                                str = "GKvbe\x00a1B";
                                break;

                            case 2:
                                str = "‡eivbe\x00a1B";
                                break;

                            case 3:
                                str = "‡Zivbe\x00a1B";
                                break;

                            case 4:
                                str = "Pyivbe\x00a1B";
                                break;

                            case 5:
                                str = "cPuvbe\x00a1B";
                                break;

                            case 6:
                                str = "‡Qqvbe\x00a1B";
                                break;

                            case 7:
                                str = "mvZvbe\x00a1B";
                                break;

                            case 8:
                                str = "AvUvbe\x00a1B";
                                break;

                            case 9:
                                str = "wbivbe\x00a1B";
                                break;
                        }
                        break;
                    }
                    str = "be\x00a1B";
                    break;
            }
        }
        return str.Trim();
    }

    private string Words_1_999(int num)
    {
        int num2 = 0;
        int num3 = 0;
        string str = null;
        num2 = num / 100;
        num3 = num - (num2 * 100);
        if (num2 > 0)
        {
            str = Words_1_99(num2) + " kZ ";
        }
        if (num3 > 0)
        {
            str = str + Words_1_99(num3);
        }
        return str.Trim();
    }

    private string Words_1_all(decimal num)
    {
        decimal[] numArray = new decimal[5];
        string[] strArray = new string[5];
        int num2 = 0;
        string str = "";
        strArray[0] = "trillion";
        numArray[0] = 1000000000000M;
        strArray[1] = "‡KvwU";
        numArray[1] = 10000000M;
        strArray[2] = "j\x00b6";
        numArray[2] = 100000M;
        strArray[3] = "nvRvi";
        numArray[3] = 1000M;
        strArray[4] = "";
        numArray[4] = 1M;
        for (int i = 0; i < 5; i++)
        {
            if (num >= numArray[i])
            {
                num2 = (int) (num / numArray[i]);
                if (str.Length > 0)
                {
                    str = str + ", ";
                }
                str = str + Words_1_999(num2) + " " + strArray[i];
                num -= num2 * numArray[i];
            }
        }
        return (str.Trim() + "  UvKv gv\x00ce|");
    }

    public string Words_Money(decimal num)
    {
        decimal num2 = 0M;
        int num3 = 0;
        string str2 = null;
        string str3 = null;
        num2 = Convert.ToInt64(num);
        str2 = Words_1_all(num2);
        if (str2.Length == 0)
        {
            str2 = "wR‡iv";
        }
        if (str2 == "GK")
        {
            str2 = str2 + "UvKv";
        }
        num3 = Convert.ToInt32((decimal) ((num - num2) * 100M));
        str3 = Words_1_all(num3);
        if (str2.Length == 0)
        {
            str3 = "wR‡iv";
        }
        if (str3 == "GK")
        {
            str3 = str3 + " cqmv";
        }
        else
        {
            str3 = str3 + " cqmv";
        }
        if (num3 > 0)
        {
            return (str2 + " Ges " + str3);
        }
        return str2;
    }
}

