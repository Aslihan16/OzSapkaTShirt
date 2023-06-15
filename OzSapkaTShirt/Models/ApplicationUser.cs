using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OzSapkaTShirt.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{

    [Column(TypeName = "nChar(30)")]
    [DisplayName("Ad")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "En fazla 30 karakter")]
    public string Name { get; set; }
    [Column(TypeName = "nChar(30)")]
    [DisplayName("Soyad")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "En fazla 30 karakter")]
    public string SurName { get; set; }

    [DisplayName("Kurumsal Hesap")]
    public bool Corporate { get; set; }

    [Column(TypeName = "nChar(200)")]
    [DisplayName("Adres")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "En fazla 200 karakter")]
    public string Address { get; set; } = default!;


    [DisplayName("Cinsiyet")]
    public byte Gender { get; set; }

    [ForeignKey("Gender")]
    public Gender? GenderType { get; set; } = default!;

    [Column(TypeName = "date")]
    [DisplayName("Doğum Tarihi")]
    [DataType(DataType.Date)] 
    public DateTime? BirthDate { get; set; }

    [Column(TypeName = "nChar(256)")]
    [DisplayName("Kullanıcı Adı")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(3000, MinimumLength = 2, ErrorMessage = "En fazla 256 karakter,en az 2 karakter zorunludur")]
    public override string UserName { get => base.UserName; set => base.UserName = value; }


    [DisplayName("Şehir")]
    public byte? CityCode { get; set; }

    [ForeignKey("CityCode")]
    public City? City { get; set; } = default!;

    [Column(TypeName = "Char(256)")]
    [DisplayName("E-posta")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(256, MinimumLength = 5, ErrorMessage = "En fazla 256 karakter, en az 5 karakter zorunludur")]
    [EmailAddress(ErrorMessage = "Geçersiz format")]
    public override string Email { get => base.Email; set => base.Email = value; }

    [Column(TypeName = "nChar(10)")]
    [DisplayName("Telefon Numarası")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "10 karakter")]
    [DataType(DataType.PhoneNumber, ErrorMessage ="Geçersiz Telefon numarası")]
    public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }


    [NotMapped]
    [DisplayName("Parola")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "En fazla 128 ,en az 8 karakter")]
    [DataType(DataType.Password)]
    public string PassWord { get; set; } = default!;


    [NotMapped]
    [DisplayName("Parola(Tekrar)")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "En fazla 128 ,en az 8 karakter")]
    [DataType(DataType.Password)]
    [Compare("PassWord", ErrorMessage = "Parola eşleşme başarısız")]
    public string ConfirmPassWord { get; set; } = default!;

    public ApplicationUser Trim()
    {
        this.Name = this.Name.Trim();
        this.SurName = this.SurName.Trim();
        this.Address = this.Address.Trim();
        this.UserName = this.UserName.Trim();
        this.Email = this.Email.Trim();
        this.PhoneNumber = this.PhoneNumber.Trim();
        return this;
    }

    [NotMapped]
    [DisplayName("Yeni Parola")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "En fazla 128 ,en az 8 karakter")]
    [DataType(DataType.Password)]
    public string NewPassWord { get; set; } = default!;


    [NotMapped]
    [DisplayName("Yeni Parola(Tekrar)")]
    [Required(ErrorMessage = "Bu alan zorunludur")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "En fazla 128 ,en az 8 karakter")]
    [DataType(DataType.Password)]
    [Compare("NewPassWord", ErrorMessage = "Parola eşleşme başarısız")]
    public string ConfirmNewPassWord { get; set; } = default!;






}

