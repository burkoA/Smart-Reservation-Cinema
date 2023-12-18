using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace SmartReservationCinema.Services
{
    public class DistanceFinder
    {
        public string APIKey { get; set; }

        public DistanceFinder(string APIKey)
        {
            this.APIKey = APIKey;
        }

        public int[] GetDistance(string origin,string[] destinations)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp("https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + origin +
                "&destinations=" + string.Join('|',destinations) + "&key="+APIKey);
            string json = getResponseStr(request);
            DistanceMatrixModel distanceMatrix = JsonSerializer.Deserialize<DistanceMatrixModel>(json);
            if (distanceMatrix.status != "OK")
            {
                throw new Exception("Map API Error!");
            }
            int[] result = distanceMatrix.rows[0].elements.Select(el => el.duration.value).ToArray();

            return result;
        }

        private string getResponseStr(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            int statusCode = (int)response.StatusCode;
            if (statusCode != 200) throw new Exception($"Ошибка получения данных statusCode={statusCode} \"{response.StatusDescription}\"");
            if (!response.ContentType.ToUpper().Contains("JSON")) throw new Exception($"Ошибка получения данных. Неожиданный тип ответа ContentType : \"{ response.ContentType }\".");

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            //str=str.Replace("\0xA", "\n");            
            reader.Close();
            stream.Close();
            response.Close();
            return result;
        }

        private class DistanceMatrixModel
        {
            public string[] destination_addresses { get; set; }
            public string[] origin_addresses { get; set; }
            public MatrixRow[] rows { get; set; }
            public string status { get; set; }
        }

        private class MatrixRow
        {
            public MatrixCell[] elements { get; set; }
        }
        private class MatrixCell
        {
            public DistanceValue distance { get; set; }
            public DistanceValue duration { get; set; }
            //public DistanceValue duration_in_traffic { get; set; }
            public string status { get; set; }
        }

        private class DistanceValue
        {
            public string text { get; set; }
            public int value { get; set; }
        }
    }
}
