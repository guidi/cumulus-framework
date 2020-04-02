using System;

namespace CumulusFramework.Util
{
    public static class ExceptionUtil
    {
        public static String TratarMensagemExcecao(Exception exception)
        {
            Exception lastException = exception.GetBaseException();
            String mensagem = String.Empty;
            if (lastException != null)
            {
                mensagem = lastException.Message;
            }
            else
            {
                mensagem = (exception.InnerException != null && !String.IsNullOrEmpty(exception.InnerException.Message) ? exception.InnerException.Message : exception.Message);
            }
            
            return mensagem;
        }
    }
}
