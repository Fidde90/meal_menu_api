namespace meal_menu_api.Models.Forms
{
    public class CreateInvitationModel
    {
        public string Email { get; set; } = null!;

        public int GroupId { get; set; }
    }
}
