using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace edu_croaker.Data
{
    public class Croak
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Author { get; set; }

        private string _content;
        public string Content 
        { 
            get => _content;
            set
            {
                _content = value;
                ExtractHashtags();
            } 
        }

        public int Shares { get; set; }

        private List<string> _hashtags = new List<string>();
        public ReadOnlyCollection<string> Hashtags => new ReadOnlyCollection<string>(_hashtags);

        protected void ExtractHashtags()
        {
            var tokens = Content.Split(' ');
            foreach (var token in tokens)
            {
                if (token.StartsWith('#'))
                {
                    _hashtags.Add(token.TrimStart('#'));
                }
            }
        }
    }
}