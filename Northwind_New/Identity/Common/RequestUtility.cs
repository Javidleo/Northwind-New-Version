using Microsoft.AspNetCore.Http;
using UAParser;


namespace Identity.Common
{
    public class RequestUtility
    {
        //HttpRequest HttpRequest;
        //public RequestUtility(HttpRequest httpRequest)
        //{
        //    HttpRequest = httpRequest;
        //}
        public static string GetBrowserInfo(HttpRequest httpRequest)
        {
            try
            {
                var userAgent = httpRequest.Headers["User-Agent"];
                var uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(userAgent);

                var Response = c.UA.Family + " " + c.UA.Major + "." + c.UA.Minor;
                return Response;
            }
            catch
            { return string.Empty; }

        }

        public string GetOsInfo(HttpRequest httpRequest)
        {
            var userAgent = httpRequest.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo c = uaParser.Parse(userAgent);

            var Response = c.OS.Family + " " + c.OS.Major + "." + c.OS.Minor;
            return Response;
        }

        public static string GetDeviceInfo(HttpRequest httpRequest)
        {
            var userAgent = httpRequest.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo c = uaParser.Parse(userAgent);

            // var Response = c.Device.Family + " " + c.Device.Brand + " " + c.Device.Model+" "+c.Device.IsSpider.ToString();
            var Response = c.ToString();
            return Response;
        }
    }
}
