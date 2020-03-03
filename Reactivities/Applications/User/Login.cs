using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Error;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Persistence;

namespace Application.User {
    public class Login {
        public class Query : IRequest<User>
        {

            public string  Email { get; set; }
            public string Password { get; set; }
        }


        public class QueryValidator:AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(res => res.Email).NotEmpty();
                RuleFor(res => res.Password).NotEmpty();
            }


        }
        public class Handler : IRequestHandler<Query, User> {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;


            public Handler (UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }
            public async Task<User> Handle (Query request, CancellationToken cancellationToken) {
                // handler logic goes here
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {

                   
                   // return null;
                    //throw new ArgumentException("incorrect details");
                     throw new RestException(HttpStatusCode.BadRequest);
                }

                var result = await  _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                   // TODOs Token Generator

                   return new User
                   {
                       UserName =  user.UserName,
                       DisplayName =  user.DisplayName,
                       Token =  _jwtGenerator.CreateToken(user),
                       Image = null
                       

                   };
                }

                return null;
                //throw new ArgumentException("incorrect details");

                throw new RestException(HttpStatusCode.BadRequest);
            }
        }
    }
}