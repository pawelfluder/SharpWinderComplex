namespace SharpTinderComplexTests.JsonObjects2
{
    public class Profile3
    {
        public string _id { get; set; }
        public int age_filter_max { get; set; }
        public int age_filter_min { get; set; }
        public List<object> badges { get; set; }
        public string bio { get; set; }
        public DateTime birth_date { get; set; }
        public string blend { get; set; }
        public bool can_create_squad { get; set; }
        public DateTime create_date { get; set; }
        public string discoverable_party { get; set; }
        public bool discoverable { get; set; }
        public int distance_filter { get; set; }
        public string email { get; set; }
        public int gender { get; set; }
        public int gender_filter { get; set; }
        public bool hide_ads { get; set; }
        public bool hide_age { get; set; }
        public bool hide_distance { get; set; }
        public List<int> interested_in { get; set; }
        public string name { get; set; }
        public List<Photo> photos { get; set; }
        public bool photo_optimizer_enabled { get; set; }
        public DateTime ping_time { get; set; }
        public Pos pos { get; set; }
        public PosInfo pos_info { get; set; }
        public bool show_gender_on_profile { get; set; }
        public object username { get; set; }
        public object jobs { get; set; }
        public object schools { get; set; }
        public object instagram { get; set; }
    }


    public class Algo
    {
        public double height_pct { get; set; }
        public double width_pct { get; set; }
        public double x_offset_pct { get; set; }
        public double y_offset_pct { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string alpha3 { get; set; }
        public string cc { get; set; }
    }

    public class CropInfo
    {
        public User user { get; set; }
        public Algo algo { get; set; }
        public bool processed_by_bullseye { get; set; }
        public bool user_customized { get; set; }
        public List<Face> faces { get; set; }
    }

    public class Dhash
    {
        public object value { get; set; }
        public string version { get; set; }
    }

    public class Face
    {
        public Algo algo { get; set; }
        public double bounding_box_percentage { get; set; }
    }

    public class Phash
    {
        public object value { get; set; }
        public string version { get; set; }
    }

    public class Photo
    {
        public List<object> assets { get; set; }
        public DateTime created_at { get; set; }
        public CropInfo crop_info { get; set; }
        public Dhash dhash { get; set; }
        public string extension { get; set; }
        public string fbId { get; set; }
        public string fileName { get; set; }
        public string id { get; set; }
        public Phash phash { get; set; }
        public List<ProcessedFile> processedFiles { get; set; }
        public int rank { get; set; }
        public double score { get; set; }
        public string type { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public List<int> webp_qf { get; set; }
        public int win_count { get; set; }
        public int jobs { get; set; }
        public object selfie_verified { get; set; }
        public object image { get; set; }
        public object thumbnail { get; set; }
        public object ts { get; set; }
        public object media_type { get; set; }
        public object processedVideos { get; set; }
        public object last_update_time { get; set; }
        public int xdistance_percent { get; set; }
        public int xoffset_percent { get; set; }
        public int ydistance_percent { get; set; }
        public int yoffset_percent { get; set; }
        public object main { get; set; }
        public object selectRate { get; set; }
        public object webp_res { get; set; }
    }

    public class Pos
    {
        public long at { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class PosInfo
    {
        public Country country { get; set; }
        public string timezone { get; set; }
    }

    public class ProcessedFile
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    
    public class User
    {
        public double height_pct { get; set; }
        public int width_pct { get; set; }
        public int x_offset_pct { get; set; }
        public double y_offset_pct { get; set; }
    }
}
