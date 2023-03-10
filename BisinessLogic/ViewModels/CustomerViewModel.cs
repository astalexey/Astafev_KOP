using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [DisplayName("Имя")]
        public string CustomerName { get; set; }
        [DisplayName("Фамилия")]
        public string CustomerSurname { get; set; }
        [DisplayName("Отчество")]
        public string Patronymic { get; set; }
        [DisplayName("Телефон")]
        public string Telephone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}
