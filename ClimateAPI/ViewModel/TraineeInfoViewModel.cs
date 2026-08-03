namespace CCDbApi.ViewModel
{
    public class TraineeInfoViewModel
    {
        public Guid? Id { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }  
        public string? DateOfBirth { get; set; } 
        public string Nationality { get; set; }

        // --- Contact Information ---
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        // --- Professional Information ---
        public string Organisation { get; set; }
        public string OrganisationType { get; set; }
        public string JobTitle { get; set; }

        // --- Payment / Billing Information ---
        public string PaymentOrganisationName { get; set; }
        public string PaymentContactPerson { get; set; }
        public string PaymentGender { get; set; }  // optional
        public string PaymentAddress { get; set; }
        public string PaymentZipCode { get; set; }
        public string PaymentCity { get; set; }
        public string PaymentCountry { get; set; }
        public string PaymentEmail { get; set; }
        public string PaymentMobilePhone { get; set; }

        // --- Discounts & Eligibility ---
        public bool IsEligibleForDiscount { get; set; }

        // --- Visa / Participation Details ---
        public bool RequiresVisa { get; set; }
        public bool HasParticipationLimitation { get; set; }
        public string ParticipationLimitationDetails { get; set; }

        // --- Consent & Subscriptions ---
        public bool AcceptsTermsAndConditions { get; set; }
        public bool SubscribeToNewsletter { get; set; }
    }
}
