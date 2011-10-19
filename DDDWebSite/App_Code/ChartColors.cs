﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Util
/// </summary>
    public class ChartColors
    {
        private string[] arr_FCColors;
        private int FC_ColorCounter;
        private Random n;
        private int iter;

        public ChartColors()
        {
            n = new Random();
            FC_ColorCounter = 0;
            iter = n.Next(0, 19);
            arr_FCColors = new string[20];
            arr_FCColors[0] = "1941A5"; //Dark Blue
            arr_FCColors[1] = "AFD8F8";
            arr_FCColors[2] = "F6BD0F";
            arr_FCColors[3] = "8BBA00";
            arr_FCColors[4] = "A66EDD";
            arr_FCColors[5] = "F984A1";
            arr_FCColors[6] = "CCCC00"; //Chrome Yellow+Green
            arr_FCColors[7] = "999999"; //Grey
            arr_FCColors[8] = "0099CC"; //Blue Shade
            arr_FCColors[9] = "FF0000"; //Bright Red 
            arr_FCColors[10] = "006F00"; //Dark Green
            arr_FCColors[11] = "0099FF"; //Blue (Light)
            arr_FCColors[12] = "FF66CC"; //Dark Pink
            arr_FCColors[13] = "669966"; //Dirty green
            arr_FCColors[14] = "7C7CB4"; //Violet shade of blue
            arr_FCColors[15] = "FF9933"; //Orange
            arr_FCColors[16] = "9900FF"; //Violet
            arr_FCColors[17] = "99FFCC"; //Blue+Green Light
            arr_FCColors[18] = "CCCCFF"; //Light violet
            arr_FCColors[19] = "669900"; //Shade of green

        }

        public string getFCColor_Random()
        {            
            int index = n.Next(0, 19);
            return arr_FCColors[index];
        }

        public string getFCColor_Iteration()
        {
            iter++;
            if (iter == 20)
                iter = 0;
            return arr_FCColors[iter];
        }
    }

