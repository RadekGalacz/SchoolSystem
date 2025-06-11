namespace AspNetCoreMVC_SchoolSystem.DTO {
    public class StudentDTO {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
