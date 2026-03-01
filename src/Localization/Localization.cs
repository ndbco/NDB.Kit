namespace NDB.Kit.Base64;
public static partial class Localization
{
    public static string ToRupiah(decimal angka)
    {
        string result = System.String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "Rp.{0:N}", angka);
        result = result.Remove(result.Length - 3);
        return result;
    }
    public static string ToThousand(decimal angka)
    {
        string result = System.String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", angka);
        result = result.Remove(result.Length - 4);
        return result;
    }
    public static string Terbilang(decimal value)
    {
        string angka = ((int)value).ToString();
        string result = "";
        for (int i = 0; i < angka.Length; i++)
        {
            string bilangan = "";
            string bilangan2 = "";
            string jumlah = "";
            int jumlahAngkaKanan = 0;

            if (angka.Substring(i, 1) != "0")
            {
                jumlahAngkaKanan = (angka.Length - 1) - i;
                if (angka.Substring(i, 1) == "1")
                {
                    try
                    {
                        if (jumlahAngkaKanan == 1 || jumlahAngkaKanan == 4 || jumlahAngkaKanan == 7)
                        {
                            if (angka.Substring(i + 1, 1).Equals("1"))
                            {
                                jumlah = "sebelas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "2")
                            {
                                jumlah = "dua belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "3")
                            {
                                jumlah = "tiga belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "4")
                            {
                                jumlah = "empat belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "5")
                            {
                                jumlah = "lima belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "6")
                            {
                                jumlah = "enam belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "7")
                            {
                                jumlah = "tujuh belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "8")
                            {
                                jumlah = "delapan belas ";
                            }
                            else if (angka.Substring(i + 1, 1) == "9")
                            {
                                jumlah = "sembilan belas ";
                            }

                            i = i + 1;
                            jumlahAngkaKanan = (angka.Length - 1) - i;
                        }
                        else if (jumlahAngkaKanan % 3 == 0 && jumlahAngkaKanan != 6)
                            jumlah = "se";
                        else
                            jumlah = "satu ";
                    }
                    //}
                    catch
                    {
                        jumlah = "satu ";
                    }


                }
                else if (angka.Substring(i, 1) == "2")
                {
                    jumlah = "dua ";
                }
                else if (angka.Substring(i, 1) == "3")
                {
                    jumlah = "tiga ";
                }
                else if (angka.Substring(i, 1) == "4")
                {
                    jumlah = "empat ";
                }
                else if (angka.Substring(i, 1) == "5")
                {
                    jumlah = "lima ";
                }
                else if (angka.Substring(i, 1) == "6")
                {
                    jumlah = "enam ";
                }
                else if (angka.Substring(i, 1) == "7")
                {
                    jumlah = "tujuh ";
                }
                else if (angka.Substring(i, 1) == "8")
                {
                    jumlah = "delapan ";
                }
                else if (angka.Substring(i, 1) == "9")
                {
                    jumlah = "sembilan ";
                }
            }
            if (jumlahAngkaKanan == 1 || jumlahAngkaKanan == 4 || jumlahAngkaKanan == 7)
                bilangan = "puluh ";
            else if (jumlahAngkaKanan == 2 || jumlahAngkaKanan == 5 || jumlahAngkaKanan == 8)
                bilangan = "ratus ";
            else
                bilangan = "";
            try
            {
                if (angka.Substring(i + 1, 1).Equals("0") && jumlahAngkaKanan % 3 != 0)
                {
                    jumlahAngkaKanan = jumlahAngkaKanan - 1;
                }
            }
            catch { }
            if (jumlahAngkaKanan == 3)
                bilangan2 = "ribu ";
            else if (jumlahAngkaKanan == 6)
                bilangan2 = "juta ";
            else if (jumlahAngkaKanan == 9)
                bilangan2 = "milyar ";
            else
                bilangan2 = "";

            result += jumlah + "" + bilangan + "" + bilangan2; ;
        }
        return result;
    }
    public static string MonthName(int month)
    {
        var Month = "Desember";
        if (month == 1)
        {
            Month = "Januari";
        }
        else if (month == 2)
        {
            Month = "Februari";
        }
        else if (month == 3)
        {
            Month = "Maret";
        }
        else if (month == 4)
        {
            Month = "April";
        }
        else if (month == 5)
        {
            Month = "Mei";
        }
        else if (month == 6)
        {
            Month = "Juni";
        }
        else if (month == 7)
        {
            Month = "Juli";
        }
        else if (month == 8)
        {
            Month = "Agustus";
        }
        else if (month == 9)
        {
            Month = "September";
        }
        else if (month == 10)
        {
            Month = "Oktober";
        }
        else if (month == 11)
        {
            Month = "November";
        }
        return Month;
    }
}