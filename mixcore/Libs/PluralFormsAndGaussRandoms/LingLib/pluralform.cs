namespace Linglib
{
    public static class pluralform
    {
        public static string nobot(int value, string [] form1_form4_form5) {
        
 string result="";
    int value1 = value % 100;
    if (value1>=11 && value1<=19) {
        result=form1_form4_form5[2];
    }
    else {
       int  i = value1 % 10;
        switch (i)
        {
            case (1): {result = form1_form4_form5[0]; break;}
            case (2):
            case (3):
            case (4): {result = form1_form4_form5[1]; break;}
            default: result = form1_form4_form5[2]; break;
        }
    }
    return result;
}
        }
        }
    

