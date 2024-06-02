using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Forms.Core.Utils
{
    public static class Utils
    {
        public static string ApenasNumeros(this string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        public static bool IsCelular(this string telefone, string ddi = null)
        {
            try
            {
                if (string.IsNullOrEmpty(telefone))
                    return false;
                //Recupera somente os digitos\
                telefone = Regex.Replace(telefone, @"[^\d]", "");

                // regras para telefones internacionais
                if (!string.IsNullOrEmpty(ddi))
                {
                    ddi = Regex.Replace(ddi, @"[^\d]", "");
                    if (ddi != "55")
                    {
                        return true;
                    }
                    else
                    {
                        //Recupera somente digitos depois do ddd
                        telefone = telefone.Substring(telefone.Length - 11);

                        //Se ddd começa com zero, remove
                        if (telefone.StartsWith("0"))
                            telefone = telefone.Remove(0, 1);

                        //Valida se é telefone valido
                        if (telefone.Length < 11)
                            return false;
                        //throw new Exception("O telefone digitado é inválido");

                        //Valida se o numero é celular tem que começar com 9
                        int phoneRange = int.Parse(telefone.Substring(2, 1));
                        if (phoneRange != 9)
                            return false;
                        //throw new Exception("O telefone digitado deve ser um celular");

                        return true;
                    }
                }
                else
                {
                    //Recupera somente digitos depois do ddd
                    telefone = telefone.Substring(telefone.Length - 11);

                    //Se ddd começa com zero, remove
                    if (telefone.StartsWith("0"))
                        telefone = telefone.Remove(0, 1);

                    //Valida se é telefone valido
                    if (telefone.Length < 11)
                        return false;
                    //throw new Exception("O telefone digitado é inválido");

                    //Valida se o numero é celular tem que começar com 9
                    int phoneRange = int.Parse(telefone.Substring(2, 1));
                    if (phoneRange != 9)
                        return false;
                    //throw new Exception("O telefone digitado deve ser um celular");

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string RemoverAcentuacao(this string texto)
        {
            var semacento = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(texto));
            return semacento;
        }

        public static string AddCodigoPaisSalvar(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return "";

            //telefone = telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();

            telefone = Regex.Replace(telefone, @"[^\d]", "");

            if (telefone.StartsWith("0"))
                telefone = telefone.Remove(0, 1);

            if (!telefone.StartsWith("55"))
                telefone = "55" + telefone;

            if (telefone.Length > 14)
                return "";

            return telefone;
        }

        public static string AddCodigoPais(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return "";

            //telefone = telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();

            telefone = Regex.Replace(telefone, @"[^\d]", "");

            if (telefone.StartsWith("0"))
                telefone = telefone.Remove(0, 1);

            if (!telefone.StartsWith("+55"))
                telefone = "+55" + telefone;

            if (telefone.Length > 14)
                return "";

            return telefone;
        }

        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string DataPesquisa(string data)
        {
            if (string.IsNullOrEmpty(data))
                return "";

            data = data.Replace("/", "-");
            return data;
        }

        public static string ConverterDataBrasileira(DateTime data)
        {
            DateTime temp;
            if (DateTime.TryParse(data.ToString(), out temp))
                return "";

            return temp.Day.ToString().PadLeft(2, '0') + "/" + temp.Month.ToString().PadLeft(2, '0') + "/" + temp.Year.ToString();
        }
        public static string ConverterDataHoraBrasileira(DateTime data)
        {
            DateTime temp;
            if (DateTime.TryParse(data.ToString(), out temp))
                return "";

            return temp.Day.ToString().PadLeft(2, '0') + "/" + temp.Month.ToString().PadLeft(2, '0') + "/" + temp.Year.ToString() + " " + temp.Hour.ToString().PadLeft(2, '0') + ":" + temp.Minute.ToString().PadLeft(2, '0') + ":" + temp.Second.ToString().PadLeft(2, '0');
        }

        public static string ValorPesquisa(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return "";

            valor = valor.Replace(",", ".");
            return valor;
        }

        public static string FormatarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return "";

            if (cpf.Length != 11)
                return cpf;

            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        public static string FormatarCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return "";

            cep = cep.Replace(".", "").Replace("-", "").Trim();

            if (cep.Length != 8)
                return cep;

            return Convert.ToUInt64(cep).ToString(@"00\.000\-000");
        }

        public static string FormatarCNPJ(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return "";

            if (cnpj.Length != 14)
                return cnpj;

            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormatarCNPJCPF(string cnpjCpf)
        {
            if (string.IsNullOrEmpty(cnpjCpf))
                return "";

            if (cnpjCpf.Length == 14)
                return Convert.ToUInt64(cnpjCpf).ToString(@"00\.000\.000\/0000\-00");
            else
                return Convert.ToUInt64(cnpjCpf).ToString(@"000\.000\.000\-00");
        }

        public static string FormatarTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return "";

            telefone = telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();

            if (telefone.Length == 10)
                telefone = long.Parse(telefone).ToString(@"(00) 0000-0000");

            if (telefone.Length == 11)
                telefone = long.Parse(telefone).ToString(@"(00) 00000-0000");

            return telefone;
        }

        public static string RemoverAcentuacaoToUpper(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            var semacento = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(texto));
            return semacento.ToUpper();
        }

        public static string ToUpperString(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            return texto.ToUpper();
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string RemoveMascaraCpfCnpj(string cpfCnpj)
        {
            if (string.IsNullOrEmpty(cpfCnpj))
                return "";

            return cpfCnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
        }

        public static string RemoveMascaraCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return "";

            return cep.Trim().Replace(".", "").Replace("-", "").Replace(" ", "");
        }

        public static string RemoveMascaraTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return "";

            return Regex.Replace(telefone, @"[^\d]", "");
        }

        public static string RetonaPrimeiroNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return "";

            var split = nome.Split(' ');
            if (split.Count() >= 1)
                return split[0];

            return nome;
        }

        public static IdentificadorAnexo GerarIdentificadorAnexo(IFormFile file)
        {
            var extensao = file.FileName.Split('.').Last();
            string identificador = $"{Guid.NewGuid()}.{extensao}";
            string tamanho = $"{ConvertBytesToKbytes(file.Length)} KB";

            return new IdentificadorAnexo(identificador, extensao, tamanho);

        }

        public static bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }

        public static double ConvertBytesToKbytes(long bytes)
        {
            return (bytes / 1024f);
        }

        public static string EncodeToBase64(this string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.ASCII.GetBytes(texto);
                string resultado = System.Convert.ToBase64String(textoAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string DecodeFrom64(this string dados)
        {
            try
            {
                byte[] dadosAsBytes = System.Convert.FromBase64String(dados);
                string resultado = System.Text.ASCIIEncoding.ASCII.GetString(dadosAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static long DateTimeToUnix(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timeSpan.TotalSeconds;
        }

        public static string DataHoraBrasil()
        {
            var timeUtc = DateTime.UtcNow;
            string dateAndHour;

            try
            {
                var brasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                dateAndHour = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasil).ToString("dd-MM-yyyy HH:mm:ss");
            }
            catch (Exception)
            {
                dateAndHour = timeUtc.ToString("dd-MM-yyyy HH:mm:ss");
            }

            return dateAndHour;
        }

        public static string RemoveSpecialChars(string field)
        {
            if (string.IsNullOrEmpty(field)) return string.Empty;
            var cleanField = Regex.Replace(field, @"[^0-9a-zA-Z:,]+", "");
            return cleanField;
        }

        public static string GenerateRandomKey(int size = 12)
        {
            var key = new byte[size];
            RandomNumberGenerator.Create().GetBytes(key);
            var secret = Convert.ToBase64String(key);
            secret = RemoveSpecialChars(secret);
            return secret;
        }

        public static string GerarSenha()
        {
            // Caracteres permitidos em cada categoria
            string letrasMaiusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string letrasMinusculas = "abcdefghijklmnopqrstuvwxyz";
            string caracteresEspeciais = "!@#$";
            string numeros = "0123456789";

            // Combine todas as categorias
            string caracteresPermitidos = letrasMaiusculas + letrasMinusculas + caracteresEspeciais + numeros;

            // Utilize um objeto Random para gerar índices aleatórios
            Random random = new Random();

            // Construa a senha
            char[] senha = new char[8];
            senha[0] = letrasMaiusculas[random.Next(letrasMaiusculas.Length)]; // Pelo menos uma letra maiúscula
            senha[1] = letrasMinusculas[random.Next(letrasMinusculas.Length)]; // Pelo menos uma letra minúscula
            senha[2] = caracteresEspeciais[random.Next(caracteresEspeciais.Length)]; // Pelo menos um caractere especial
            senha[3] = numeros[random.Next(numeros.Length)]; // Pelo menos um número

            // Preencha o restante da senha com caracteres aleatórios
            for (int i = 4; i < senha.Length; i++)
            {
                senha[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];
            }

            // Embaralhe a ordem dos caracteres na senha
            for (int i = senha.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = senha[i];
                senha[i] = senha[j];
                senha[j] = temp;
            }

            return new string(senha);
        }
        public static string GerarCodigo(int comprimento)
        {
            Random random = new Random();
            StringBuilder codigoBuilder = new StringBuilder(comprimento);

            for (int i = 0; i < comprimento; i++)
            {
                codigoBuilder.Append(random.Next(10));
            }

            return codigoBuilder.ToString();
        }

        public static IFormFile ConvertBase64ToIFormFile(string base64String, string fileName, string contentType)
        {
            // Decodifica a string Base64 para um array de bytes
            byte[] fileBytes = Convert.FromBase64String(base64String);

            // Cria um objeto IFormFile a partir dos bytes do arquivo
            IFormFile formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
        }

        public static Stream ConvertBase64ToStream(string base64String)
        {
            // Decodifica a string Base64 para um array de bytes
            byte[] fileBytes = Convert.FromBase64String(base64String);

            // Cria um objeto MemoryStream a partir dos bytes do arquivo
            Stream stream = new MemoryStream(fileBytes);

            return stream;
        }

        public static DateTime GetDate()
        {
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, brasilia);
        }
    }
}
