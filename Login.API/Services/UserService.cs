using System.Collections.Generic;
using System.Security.Cryptography;
using Login.API.Data;
using Login.API.Models;
using Login.API.Request;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Login.API.Services
{
    public class UserService
    {
        private readonly LoginContext _context;
        public UserService(LoginContext context)
        {
            _context = context;
        }
        //Método para criar hash da senha
        public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //deriva uma chave da senha
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Guarda salt + hash para depois verificar
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        //verifica se a senha bate com o hash salvo
        public bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var savedHash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed == savedHash;
        }


        public async Task<string> Register(RegisterRequestDto request)
        {
            try
            {
                // Verifica se o Email j� existe
                if (await _context.TblUser.AnyAsync(u => u.Email == request.Email))
                    throw new Exception("Email já está em uso.");

                UserClass user = new UserClass();
                user.Email = request.Email;
                user.Username = request.Username;
                user.PasswordHash = HashPassword(request.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.EmailConfirmed = false;
                user.EmailConfirmationToken = Guid.NewGuid().ToString();
                user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);
                user.PasswordResetToken = Guid.NewGuid().ToString();
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);

                _context.TblUser.Add(user);
                await _context.SaveChangesAsync();

                return "Usuário registrado com sucesso!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Login(LoginRequestDto request)
        {
            try
            {
                var user = await _context.TblUser.FirstOrDefaultAsync(u => u.Email == request.Identifier || u.Username == request.Identifier);

                if (user == null)
                    throw new Exception("Usuário não encontrado.");

                if (!VerifyPassword(request.Password, user.PasswordHash))
                    throw new Exception("Senha inválida.");
                
                return "Login realizado com sucesso!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
