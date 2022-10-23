using System;

public class NumberToWords
{
    private int s = 1;

    private string changeToWords(string numb, bool isCurrency)
    {
        string str = "";
        string number = numb;
        string str3 = "";
        string str4 = "";
        string str5 = "";
        string str6 = isCurrency ? "Taka Only" : "";
        try
        {
            int index = numb.IndexOf(".");
            if (index > 0)
            {
                number = numb.Substring(0, index);
                str3 = numb.Substring(index + 1);
                if (Convert.ToInt32(str3) > 0)
                {
                    str4 = isCurrency ? "Taka and " : "point";
                    str6 = isCurrency ? ("Paisa " + str6) : "";
                    str5 = translateCents(str3);
                }
            }
            str = string.Format("{0} {1}{2} {3}", new object[] { translateWholeNumber(number).Trim(), str4, str5, str6 });
        }
        catch
        {
        }
        return str;
    }

    public string NumberInWords(string AmountInNumber)
    {
        return changeToWords(AmountInNumber, true);
    }

    private string ones(string digit)
    {
        int num = Convert.ToInt32(digit);
        switch (num)
        {
            case 1:
                return "One";

            case 2:
                return "Two";

            case 3:
                return "Three";

            case 4:
                return "Four";

            case 5:
                return "Five";

            case 6:
                return "Six";

            case 7:
                return "Seven";

            case 8:
                return "Eight";

            case 9:
                return "Nine";
        }
        return "";
    }

    private string tens(string digit)
    {
        int num = Convert.ToInt32(digit);
        string str = null;
        switch (num)
        {
            case 10:
                return "Ten";

            case 11:
                return "Eleven";

            case 12:
                return "Twelve";

            case 13:
                return "Thirteen";

            case 14:
                return "Fourteen";

            case 15:
                return "Fifteen";

            case 0x10:
                return "Sixteen";

            case 0x11:
                return "Seventeen";

            case 0x12:
                return "Eighteen";

            case 0x13:
                return "Nineteen";

            case 20:
                return "Twenty";

            case 30:
                return "Thirty";

            case 40:
                return "Forty";

            case 50:
                return "Fifty";

            case 80:
                return "Eighty";

            case 90:
                return "Ninety";

            case 60:
                return "Sixty";

            case 70:
                return "Seventy";
        }
        if (num > 0)
        {
            str = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
        }
        if (num == 0)
        {
            s = 0;
        }
        return str;
    }

    private string translateCents(string cents)
    {
        string str = "";
        if (cents.Length == 1)
        {
            cents = cents.PadRight(2, '0');
        }
        if (str.Equals("0"))
        {
            return "Zero";
        }
        return tens(cents);
    }

    private string translateWholeNumber(string number)
    {
        string str = "";
        try
        {
            bool flag = false;
            bool flag2 = false;
            if (Convert.ToDouble(number) != 0.0)
            {
                flag = number.StartsWith("0");
                int length = number.Length;
                int num3 = 0;
                string str2 = "";
                switch (length)
                {
                    case 1:
                        str = ones(number);
                        flag2 = true;
                        break;

                    case 2:
                        str = tens(number);
                        flag2 = true;
                        break;

                    case 3:
                        num3 = (length % 3) + 1;
                        str2 = " Hundred ";
                        break;

                    case 4:
                        num3 = (length % 4) + 1;
                        str2 = " Thousand ";
                        break;

                    case 5:
                        num3 = (length % 4) + 1;
                        str2 = " Thousands ";
                        break;

                    case 6:
                        num3 = (length % 6) + 1;
                        str2 = " Lac ";
                        break;

                    case 7:
                        num3 = (length % 6) + 1;
                        str2 = " Lacs ";
                        break;

                    case 8:
                    case 9:
                        num3 = (length % 8) + 1;
                        str2 = " Crore ";
                        break;

                    case 10:
                        num3 = (length % 8) + 1;
                        str2 = " Crore ";
                        break;

                    default:
                        flag2 = true;
                        break;
                }
                if (!flag2)
                {
                    str = translateWholeNumber(number.Substring(0, num3)) + str2 + translateWholeNumber(number.Substring(num3));
                    if (flag)
                    {
                        str = "  and " + str.Trim();
                    }
                }
                if (str.Trim().Equals(str2.Trim()))
                {
                    str = "";
                }
            }
        }
        catch
        {
        }
        return str.Trim();
    }
}

