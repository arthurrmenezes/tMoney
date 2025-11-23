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
                        <a href=""{emailConfirmationLink}"" class=""button"">Confirmar conta</a>
                    </p>

                    <p class=""text"">
                        ⚠️ Este link é válido por <strong>24 horas</strong>.
                    </p>

                    <p class=""text"">
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney 💚</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string WelcomeEmailTemplateSubject()
    {
        return "Bem-vindo(a) à tMoney! Confirme seu e-mail para começar 🚀";
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
                        Aqui está um novo link para confirmar seu e-mail.<br>
                        Clique no botão abaixo para ativar sua conta e começar a usar todos os nossos recursos:
                    </p>

                    <p style=""text-align:center;"">
                        <a href=""{emailConfirmationLink}"" class=""button"">Confirmar conta</a>
                    </p>

                    <p class=""text"">
                        ⚠️ Este link é válido por <strong>24 horas</strong>.<br>
                        Se você já confirmou seu e-mail recentemente, pode desconsiderar esta mensagem.
                    </p>

                    <p class=""text"">
                        Qualquer dúvida, estamos aqui para ajudar.<br>
                        <strong>Equipe tMoney 💚</strong>
                    </p>
                </div>
            </body>
        </html>";
    }

    public static string ResendConfirmationEmailTemplateSubject()
    {
        return $"Confirme seu e-mail - tMoney 💵";
    }
}
