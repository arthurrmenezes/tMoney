using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Services.EmailService.Domain;

public static class EmailTemplates
{
    public static string WelcomeEmailTemplateMessageBody(string firstName, string emailConfirmationLink)
    {
        return $@"
            <!DOCTYPE html>
            <html lang=""pt-br"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <style>
                    body {{
                        font-family: Arial, Helvetica, sans-serif;
                        background-color: #f4f4f7;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    }}
                    .title {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #333333;
                        margin-bottom: 10px;
                    }}
                    .text {{
                        font-size: 16px;
                        color: #555555;
                        line-height: 1.6;
                    }}
                    .button {{
                        display: inline-block;
                        margin-top: 20px;
                        padding: 14px 24px;
                        background-color: #007bff;
                        color: #ffffff !important;
                        text-decoration: none;
                        border-radius: 6px;
                        font-weight: bold;
                    }}
                    .footer {{
                        margin-top: 30px;
                        font-size: 14px;
                        color: #777777;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <p class=""title"">Olá, {FirstNameValueObject.Factory(firstName)}! 👋</p>

                    <p class=""text"">
                        Seja muito bem-vindo(a) à <strong>tMoney</strong>! 🎉<br>
                        Sua conta foi criada com sucesso e estamos felizes em tê-lo conosco.
                    </p>

                    <p class=""text"">
                        Para ativar sua conta e começar a usar todos os recursos, confirme seu e-mail clicando no botão abaixo:
                    </p>

                    <p style=""text-align:center;"">
                        {emailConfirmationLink}
                    </p>

                    <p class=""text"">
                        ⚠️ Este link é válido por <strong>24 horas</strong>.
                    </p>

                    <p class=""text"">
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string WelcomeEmailTemplateSubject()
    {
        return "Bem-vindo(a) à tMoney! Confirme seu e-mail para começar";
    }

    public static string ResendConfirmationEmailTemplateMessageBody(string firstName, string emailConfirmationLink)
    {
        return $@"
            <!DOCTYPE html>
            <html lang=""pt-br"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <style>
                    body {{
                        font-family: Arial, Helvetica, sans-serif;
                        background-color: #f4f4f7;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    }}
                    .title {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #333333;
                        margin-bottom: 10px;
                    }}
                    .text {{
                        font-size: 16px;
                        color: #555555;
                        line-height: 1.6;
                    }}
                    .button {{
                        display: inline-block;
                        margin-top: 20px;
                        padding: 14px 24px;
                        background-color: #007bff;
                        color: #ffffff !important;
                        text-decoration: none;
                        border-radius: 6px;
                        font-weight: bold;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <p class=""title"">Olá, {FirstNameValueObject.Factory(firstName)}! 👋</p>

                    <p class=""text"">
                        Aqui está um novo link para confirmar seu e-mail.<br>
                        Clique no botão abaixo para ativar sua conta e começar a usar todos os nossos recursos:
                    </p>

                    <p style=""text-align:center;"">
                        {emailConfirmationLink}
                    </p>

                    <p class=""text"">
                        ⚠️ Este link é válido por <strong>24 horas</strong>.<br>
                        Se você já confirmou seu e-mail recentemente, pode desconsiderar esta mensagem.
                    </p>

                    <p class=""text"">
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string ResendConfirmationEmailTemplateSubject()
    {
        return $"Confirme seu e-mail - tMoney";
    }

    public static string ChangePasswordTemplateMessageBody(string firstName, DateTime dateTime)
    {
        var formattedDate = dateTime.ToString("dd/MM/yyyy");
        var formattedTime = dateTime.ToString("HH:mm");

        return $@"
            <!DOCTYPE html>
            <html lang=""pt-br"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <style>
                    body {{
                        font-family: Arial, Helvetica, sans-serif;
                        background-color: #f4f4f7;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    }}
                    .title {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #333333;
                        margin-bottom: 10px;
                    }}
                    .text {{
                        font-size: 16px;
                        color: #555555;
                        line-height: 1.6;
                    }}
                    .footer {{
                        margin-top: 30px;
                        font-size: 14px;
                        color: #777777;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <p class=""title"">Olá, {FirstNameValueObject.Factory(firstName)}!</p>

                    <p class=""text"">
                        Informamos que a senha da sua conta foi alterada com sucesso.<br>
                    </p>

                    <div class=""alert-box"">
                        <strong>Detalhes da alteração:</strong><br>
                        📅 Data: {formattedDate} às {formattedTime}<br>
                    </div>

                    <p class=""text"">
                        Se foi você quem realizou esta alteração, pode ignorar este e-mail.<br>
                        É apenas um aviso de segurança.
                    </p>
                    
                    <hr style=""border: 0; border-top: 1px solid #eee; margin: 20px 0;"">

                    <p class=""text"">
                        ❗ Mas se você <strong>não reconhece esta alteração</strong>, sua conta pode estar em risco!<br>
                        Recomendamos que:<br>
                        1. <strong>Redefina sua senha imediatamente:</strong><br>
                        http://localhost:5173/forgot-password<br>
                        2. Entre em contato com nosso suporte.
                    </p>

                    <p class=""text"">
                        A segurança da sua conta é nossa prioridade.<br>
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string ChangePasswordTemplateSubject()
    {
        return $"Alteração de senha confirmada - tMoney";
    }

    public static string ForgotPasswordTemplateMessageBody(string firstName, string resetPasswordLink)
    {
        return $@"
            <!DOCTYPE html>
            <html lang=""pt-br"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <style>
                    body {{
                        font-family: Arial, Helvetica, sans-serif;
                        background-color: #f4f4f7;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    }}
                    .title {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #333333;
                        margin-bottom: 10px;
                    }}
                    .text {{
                        font-size: 16px;
                        color: #555555;
                        line-height: 1.6;
                    }}
                    .button {{
                        display: inline-block;
                        margin-top: 20px;
                        padding: 14px 24px;
                        background-color: #007bff;
                        color: #ffffff !important;
                        text-decoration: none;
                        border-radius: 6px;
                        font-weight: bold;
                    }}
                    .footer {{
                        margin-top: 30px;
                        font-size: 14px;
                        color: #777777;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <p class=""title"">Olá, {FirstNameValueObject.Factory(firstName)}!</p>

                    <p class=""text"">
                        Recebemos uma solicitação para redefinir a senha da sua conta.<br>
                        Para continuar, clique no link abaixo e cadastre uma nova senha:
                    </p>

                    <p style=""text-align:center;"">
                        <strong>Redefinir senha:</strong><br>
                        {resetPasswordLink}
                    </p>

                    <p class=""text"">
                        ⚠️ Este link é válido por <strong>30 minutos</strong>.
                    </p>

                    <p class=""text"">
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string ForgotPasswordTemplateSubject()
    {
        return $"Recuperação de senha - tMoney";
    }

    public static string WelcomeEmailGoogleAuthTemplateMessageBody(string firstName)
    {
        return $@"
            <!DOCTYPE html>
            <html lang=""pt-br"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <style>
                    body {{
                        font-family: Arial, Helvetica, sans-serif;
                        background-color: #f4f4f7;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        padding: 30px;
                        border-radius: 8px;
                        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                    }}
                    .title {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #333333;
                        margin-bottom: 10px;
                    }}
                    .text {{
                        font-size: 16px;
                        color: #555555;
                        line-height: 1.6;
                    }}
                    .button {{
                        display: inline-block;
                        margin-top: 20px;
                        padding: 14px 24px;
                        background-color: #007bff;
                        color: #ffffff !important;
                        text-decoration: none;
                        border-radius: 6px;
                        font-weight: bold;
                    }}
                    .footer {{
                        margin-top: 30px;
                        font-size: 14px;
                        color: #777777;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <p class=""title"">Olá, {FirstNameValueObject.Factory(firstName)}! 👋</p>

                    <p class=""text"">
                        Seja muito bem-vindo(a) à <strong>tMoney</strong>! 🎉<br>
                        Sua conta foi criada com sucesso e estamos felizes em tê-lo conosco.
                    </p>

                    <p class=""text"">
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string WelcomeEmailGoogleAuthTemplateSubject()
    {
        return "Bem-vindo(a) à tMoney! Conta criada com sucesso!";
    }
}
