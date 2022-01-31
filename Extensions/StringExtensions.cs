namespace MDDPlatform.Messages.Broker.Exctensions{
    public static class StringExtension
    {
        public static List<string> GetPlaceHolders(this string input, char openSymbole = '{',char closeSymbole='}'){
            List<string> placeholders = new List<string>();
            Stack<string> blocks = new Stack<string>();
            int length = input.Length;
            string blockText = string.Empty;
            for(int i=0;i<length;i++){
                var ch = input[i];
                if(ch.IsOpenSymbole())
                {
                    blocks.Clear();
                    blocks.Push(ch.ToString());
                }
                if(ch.IsCloseSymbole()){
                    blockText = ch.ToString();
                    bool openSymboleIsFounded = false;
                    while(!blocks.IsEmpty() && !openSymboleIsFounded){
                        var popedSymbole = blocks.Pop();
                        blockText = popedSymbole + blockText;
                        if(popedSymbole.IsOpenSymbole()) openSymboleIsFounded = true;                        
                    }
                    if(openSymboleIsFounded && blockText.IsValidPlaceholder()) placeholders.Add(blockText);                    
                }
                if(ch.IsNotOpenOrCloseSymbole()) blocks.Push(ch.ToString());
            }
            return placeholders;
        }

        public static bool IsOpenSymbole(this char symbole,char openSymbole = '{'){
            return (symbole== openSymbole);
        }
        public static bool IsCloseSymbole(this char symbole,char closeSymbole='}'){
            return (symbole == closeSymbole);
        }
        public static bool IsNotOpenOrCloseSymbole(this char symbole,char openSymbole = '{',char closeSymbole='}'){
            if(symbole.IsOpenSymbole(openSymbole)) return false;
            if(symbole.IsCloseSymbole(closeSymbole)) return false;

            return true;            
        }

        public static bool IsOpenSymbole(this string symbole,string openSymbole = "{"){
            return (symbole== openSymbole);
        }
        public static bool IsCloseSymbole(this string symbole,string closeSymbole="}"){
            return (symbole == closeSymbole);
        }
        public static bool IsEmpty<T>(this Stack<T> stack){
                return (stack.Count ==0);
        }

        public static bool IsValidPlaceholder(this string blockText,char openSymbole = '{',char closeSymbole='}'){
            if(blockText==null) return false;
            if(blockText== string.Empty) return false;
            int length = blockText.Length;

            if(!blockText[0].IsOpenSymbole()) return false;
            if(!blockText[length-1].IsCloseSymbole()) return false;

            if(length==2) return false;

            var insideText = blockText.Substring(1,length-2);
            insideText = insideText.Trim();
            if(insideText.Length==0) return false;
            if(insideText.Contains(openSymbole) || insideText.Contains(closeSymbole)) return false;

            return true;            
        }

        public static string? ExtractPlaceholderVariable(this string blockText){
            if(blockText==null) return null;
            if(blockText== string.Empty) return null;
            int length = blockText.Length;

            if(!blockText[0].IsOpenSymbole()) return null;
            if(!blockText[length-1].IsCloseSymbole()) return null;

            if(length==2) return null;

            var insideText = blockText.Substring(1,length-2);
            insideText = insideText.Trim();
            if(insideText.Length==0) return null;

            return insideText;            
        }
    }
}