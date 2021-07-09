﻿using ClothesShoppingWebApp.Models;
using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesShoppingWebApp.Controllers
{
    public class SignupController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index(string? email, string? fullname)
        {
            UserEditModel userEditModel = new UserEditModel()
            {
                Email = email,
                FullName = fullname
            };
            return View(userEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserEditModel user)
        {
            bool isError = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                string password = user.Password;
                string confirm = user.Confirm;
                if (!password.Equals(confirm))
                {
                    isError = true;
                    message = "Confirm and Password are not matched!!";
                } else
                {
                    string email = user.Email;
                    string fullname = user.FullName;
                    bool status = true; // Active
                    int role = 2; // User

                    try
                    {
                        DTOLibrary.User newUser = new DTOLibrary.User()
                        {
                            FullName = fullname,
                            Email = email,
                            Password = password,
                            Role = role,
                            Status = status
                        };

                        IUserRepository userRepository = new UserRepository();
                        userRepository.SignUp(newUser);

                    } catch (Exception ex)
                    {
                        isError = true;
                        message = ex.Message;
                    }                    
                }
            }

            if (isError)
            {
                ViewBag.Error = message;
                return View(user);
            }
            TempData["SignUp"] = "Sign up successfully! You can login now!";
            return RedirectToAction("Index", "Login");
        }

        [AllowAnonymous]
        public IActionResult Google()
        {
            return RedirectToAction("Google", "Login");
        }
    }
}
