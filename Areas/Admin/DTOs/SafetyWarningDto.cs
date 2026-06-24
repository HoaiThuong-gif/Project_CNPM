namespace Project_CNPM.Area.Admin.DTOs
{
    // --- DỊ ỨNG (ALLERGY) ---
    public class AllergyCreateUpdateDto
    {
        public int AllergyId { get; set; } 
        public string AllergyName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // --- BỆNH NỀN (BACKGROUND DISEASE) ---
    public class BackgroundDiseaseCreateUpdateDto
    {
        public int BackgroundDiseaseId { get; set; } 
        public string DiseaseName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // --- TƯƠNG TÁC THUỐC (DRUG INTERACTION) ---
    public class DrugInteractionCreateDto
    {
        public int Medicine1Id { get; set; }
        public int Medicine2Id { get; set; }
        public int SeverityLevel { get; set; } // 1: Nhẹ, 2: Trung bình, 3: Nghiêm trọng
        public string Description { get; set; } = string.Empty;
    }

    public class DrugInteractionDetailDto : DrugInteractionCreateDto
    {
        public string Medicine1Name { get; set; } = string.Empty;
        public string Medicine2Name { get; set; } = string.Empty;
    }
}