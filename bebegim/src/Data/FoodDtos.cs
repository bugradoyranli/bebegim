     
     public class CustomFeedingRecordDto
    
{
        public int KidId { get; set; }
        public string FoodName { get; set; } // Kullanıcının yazdığı yeni isim
        public string? Description { get; set; } // Yemek açıklaması
        public decimal Amount { get; set; }
        public string Unit { get; set; }
        public string? Detail { get; set; }
        public DateTime? Date { get; set; }
    }



        public class FeedingRecordDto
    {
        public int KidId { get; set; }
        public int FoodId { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }
        public string? Detail { get; set; }
        public DateTime? Date { get; set; }
    }


        public class FoodCreateDto
    {
        public int KidId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }   