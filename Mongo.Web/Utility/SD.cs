﻿namespace Mongo.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase {  get; set; }
        public static string AuthAPIBase { get; set; }

        public static string ProductAPIBase { get; set; }

        public enum ApiType
        {
            GET,
            POST, 
            PUT,
            DELETE,
        }
    }
}
