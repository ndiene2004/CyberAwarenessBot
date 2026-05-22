using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberAwarenessBot
{
    public partial class MainWindow : Window
    {
    
        // QUESTION 8 — CODE OPTIMISATION
        // Using: Dictionary, List, Random, OOP (methods and classes)
         

        private readonly Random _random = new Random();

        // MEMORY: stores things the user tells us (Q5)
        private readonly Dictionary<string, string> _memory = new Dictionary<string, string>();

        // Tracks the last topic the user asked about (Q4 - conversation flow)
        private string _lastTopic = "";

        // Counts how many messages have been sent
        private int _messageCount = 0;

        
        // QUESTION 2 + QUESTION 3
        // KEYWORD RECOGNITION + RANDOM RESPONSES
        // Dictionary maps each keyword to a LIST of possible responses
        // The bot randomly picks one response from the list each time
        
        private readonly Dictionary<string, List<string>> _responses = new Dictionary<string, List<string>>
        {
            {
                "password", new List<string>
                {
                    "🔐 Use strong, unique passwords for each account. Avoid using your name or birthday!",
                    "🔑 A strong password has at least 12 characters with uppercase, lowercase, numbers and symbols.",
                    "🛡 Never reuse passwords across multiple websites. One breach could expose all your accounts!",
                    "💡 Use a reputable password manager to generate and safely store complex passwords.",
                    "⚠️ Change your passwords regularly and never share them with anyone — not even IT support!"
                }
            },
            {
                "phishing", new List<string>
                {
                    "🎣 Phishing emails pretend to be from trusted companies. Always check the sender's real email address!",
                    "📧 Never click suspicious links in emails. Go directly to the website by typing the URL yourself.",
                    "⚠️ Scammers create urgency — 'Act now or your account closes!' — do not fall for pressure tactics.",
                    "🔍 Hover over links before clicking to see where they really lead. Fake URLs are a big red flag!",
                    "📞 If unsure, call the company using a number from their official website — never from the email."
                }
            },
            {
                "scam", new List<string>
                {
                    "🚨 If an offer seems too good to be true, it almost certainly is. Trust your instincts!",
                    "💳 Never send money or gift cards to someone you have not met in person — a classic scam tactic.",
                    "📱 Romance scams, lottery scams, and tech-support scams are all rising. Stay alert!",
                    "🔎 Verify any unsolicited contact independently before sharing any personal information.",
                    "🏦 Banks will NEVER ask for your PIN or password over the phone or by email."
                }
            },
            {
                "privacy", new List<string>
                {
                    "👁 Review privacy settings on your social media. Limit who can see your personal information.",
                    "🌐 Be careful what you share online — once posted, it can be very hard to remove completely.",
                    "📲 Check app permissions. Does a flashlight app really need access to your contacts and camera?",
                    "🔒 Use a VPN on public Wi-Fi networks to protect your data from eavesdroppers.",
                    "🗂 Regularly check which apps have access to your Google or social media accounts."
                }
            },
            {
                "malware", new List<string>
                {
                    "🦠 Keep your OS and software updated — patches fix security vulnerabilities exploited by malware.",
                    "💾 Never download software from unofficial or untrusted websites. Stick to official sources only.",
                    "🛡 Install reputable antivirus software and run regular scans on all your devices.",
                    "📎 Be cautious with email attachments — even from known contacts whose accounts may be hacked.",
                    "🚫 Avoid pirated software — it is a common delivery method for malware and ransomware."
                }
            },
            {
                "virus", new List<string>
                {
                    "🦠 Viruses spread through infected files, emails, and downloads. Always scan files before opening.",
                    "🛡 Good antivirus software running in real-time is your first line of defence against viruses.",
                    "💡 Keep your software patched — many viruses exploit known vulnerabilities in outdated programs.",
                    "🔄 Regular backups mean a virus attack will not result in permanent data loss.",
                    "📧 Email attachments are one of the most common ways viruses are spread between users."
                }
            },
            {
                "2fa", new List<string>
                {
                    "📲 Two-Factor Authentication (2FA) adds a second layer of security beyond just your password.",
                    "🔐 Enable 2FA on all important accounts — especially email, banking, and social media.",
                    "✅ Authenticator apps like Google Authenticator are more secure than SMS-based 2FA codes.",
                    "🛡 Even if a hacker gets your password, 2FA stops them without the second factor.",
                    "💡 Setting up 2FA takes only a few minutes and dramatically improves your account security."
                }
            },
            {
                "ransomware", new List<string>
                {
                    "💰 Ransomware encrypts your files and demands payment. Regular backups are your best defence!",
                    "🚫 Never pay the ransom — there is no guarantee you will get your data back.",
                    "💾 Keep offline backups of important data. Ransomware can also encrypt cloud-synced files.",
                    "🛡 Keep software updated and use reputable antivirus to block most ransomware attacks.",
                    "📧 Ransomware often arrives via phishing emails. Think carefully before clicking any attachment!"
                }
            },
            {
                "wifi", new List<string>
                {
                    "📶 Public Wi-Fi is unencrypted. Avoid banking or sensitive accounts on public networks.",
                    "🔒 Always use a VPN on public Wi-Fi to encrypt your internet traffic and protect your data.",
                    "🏠 Change your home router default admin password — default credentials are publicly known!",
                    "📡 Use WPA3 or WPA2 encryption on your home Wi-Fi network, not the outdated WEP standard.",
                    "👀 Watch out for evil twin Wi-Fi hotspots — fake networks set up by hackers to steal your data."
                }
            },
            {
                "social engineering", new List<string>
                {
                    "🎭 Social engineering manipulates people psychologically rather than hacking systems directly.",
                    "📞 Always verify the identity of someone claiming to be IT support before granting any access.",
                    "🏢 Tailgating — following someone through a secured door — is a common physical attack method.",
                    "🧠 Attackers often build trust over time before making their malicious request. Stay sceptical.",
                    "🔐 No legitimate organisation will ever ask for your password to fix your account."
                }
            }
        };

        
        // QUESTION 6 — SENTIMENT DETECTION
        // Simple keyword-based sentiment lists
        
        private readonly List<string> _worriedWords = new List<string> { "worried", "scared", "afraid", "panic", "nervous", "anxious", "terrified", "fear" };
        private readonly List<string> _curiousWords = new List<string> { "curious", "wonder", "interested", "how does", "what is", "explain", "tell me", "learn" };
        private readonly List<string> _frustratedWords = new List<string> { "frustrated", "annoyed", "angry", "useless", "hate", "stupid", "confused", "don't understand" };
        private readonly List<string> _happyWords = new List<string> { "thanks", "great", "awesome", "helpful", "perfect", "excellent", "love", "good" };

        // Greetings list
        private readonly List<string> _greetings = new List<string> { "hi", "hello", "hey", "good morning", "good afternoon", "good evening", "howdy" };

        // Farewell list
        private readonly List<string> _farewells = new List<string> { "bye", "goodbye", "exit", "quit", "see you", "farewell", "later" };

        
        // QUESTION 4 — CONVERSATION FLOW
        // Follow-up trigger words so user can ask for more on same topic
        
        private readonly List<string> _followUps = new List<string> { "more", "tell me more", "another", "give me another", "explain more", "go on", "continue", "again" };

        
        // QUESTION 7 — ERROR HANDLING
        // Default fallback responses for unknown input
        
        private readonly List<string> _unknownResponses = new List<string>
        {
            "🤔 I am not sure I understand that. Try rephrasing, or type 'help' to see what I can assist with!",
            "🛡 I specialise in cybersecurity topics. Try asking about passwords, phishing, scams, or privacy!",
            "🔍 I did not quite catch that. Could you rephrase? Type 'help' to see a list of topics I know!",
            "💡 That is outside my expertise. I focus on cybersecurity awareness. Type 'help' to see all topics!"
        };

        
        // CONSTRUCTOR — runs when the window first opens
        
        public MainWindow()
        {
            InitializeComponent();
            ShowWelcomeMessage();
        }

        
        // WELCOME MESSAGE — shown when the app first starts
        
        private void ShowWelcomeMessage()
        {
            AddBotMessage(
                "👋 Hello! I am CyberGuard, your Cybersecurity Awareness Assistant!\n\n" +
                "I can help you with topics like:\n" +
                "  🔐 Passwords      🎣 Phishing      🚨 Scams\n" +
                "  👁 Privacy        🦠 Malware       📲 2FA\n" +
                "  💰 Ransomware     📶 Wi-Fi         🎭 Social Engineering\n\n" +
                "What is your name? I would love to personalise our chat!");

            StatusLabel.Text = "💬 Waiting for your name...";
        }

        
        // EVENT: Send button clicked
        
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

       
        // EVENT: Enter key pressed in the text box
       
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessInput();
            }
        }

        
        // EVENT: Clear button clicked — resets the whole chat
        
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            _memory.Clear();
            _lastTopic = "";
            _messageCount = 0;
            UpdateMemoryLabel();
            StatusLabel.Text = "🗑 Chat cleared. Starting fresh...";
            ShowWelcomeMessage();
        }

        
        // MAIN METHOD — reads user input and decides what response to give
        
        private void ProcessInput()
        {
            // Read what the user typed
            string input = UserInputBox.Text.Trim();

            // Do nothing if the input box is empty
            if (string.IsNullOrWhiteSpace(input)) return;

            // Show the user's message on screen
            AddUserMessage(input);

            // Clear the input box ready for next message
            UserInputBox.Clear();

            // Count the message
            _messageCount++;

            // Convert to lowercase so keyword matching is not case-sensitive
            string lower = input.ToLower();

            // Detect the user's sentiment/mood (Q6)
            string sentiment = DetectSentiment(lower);

            // Update the mood bar on screen (Q6)
            UpdateSentimentBar(sentiment);

            // Generate the bot's response
            string response = GenerateResponse(lower, input, sentiment);

            // Show the bot's response on screen
            AddBotMessage(response);

            // Update the status bar at the bottom
            StatusLabel.Text = $"💬 Message #{_messageCount} — Last topic: {(_lastTopic == "" ? "None" : _lastTopic)}";
            UpdateMemoryLabel();

            // Scroll to the latest message
            ChatScrollViewer.ScrollToBottom();
        }

        // QUESTION 6 — SENTIMENT DETECTION METHOD
        // Checks what mood/feeling words the user used
        
        private string DetectSentiment(string lower)
        {
            foreach (string word in _worriedWords)
                if (lower.Contains(word)) return "worried";

            foreach (string word in _frustratedWords)
                if (lower.Contains(word)) return "frustrated";

            foreach (string word in _happyWords)
                if (lower.Contains(word)) return "happy";

            foreach (string word in _curiousWords)
                if (lower.Contains(word)) return "curious";

            return "neutral";
        }

        // Updates the mood progress bar colour and label
        private void UpdateSentimentBar(string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    SentimentBar.Value = 25;
                    SentimentBar.Foreground = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                    SentimentLabel.Text = "Worried 😟";
                    break;
                case "frustrated":
                    SentimentBar.Value = 10;
                    SentimentBar.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                    SentimentLabel.Text = "Frustrated 😤";
                    break;
                case "happy":
                    SentimentBar.Value = 90;
                    SentimentBar.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 136));
                    SentimentLabel.Text = "Happy 😊";
                    break;
                case "curious":
                    SentimentBar.Value = 70;
                    SentimentBar.Foreground = new SolidColorBrush(Color.FromRgb(100, 200, 255));
                    SentimentLabel.Text = "Curious 🤔";
                    break;
                default:
                    SentimentBar.Value = 50;
                    SentimentBar.Foreground = new SolidColorBrush(Color.FromRgb(139, 148, 158));
                    SentimentLabel.Text = "Neutral 😐";
                    break;
            }
        }

        
        // GENERATE RESPONSE METHOD
        // This is the brain of the chatbot — decides what to say
        
        private string GenerateResponse(string lower, string original, string sentiment)
        {
            
            // QUESTION 5 — MEMORY: Try to learn the user's name on first message
            
            if (!_memory.ContainsKey("name") && _messageCount == 1)
            {
                string possibleName = ExtractName(original);

                if (!string.IsNullOrEmpty(possibleName))
                {
                    _memory["name"] = possibleName;
                }
                else
                {
                    // If no pattern like "my name is X", just take the first word as the name
                    _memory["name"] = original.Split(' ')[0];
                }

                return $"Nice to meet you, {_memory["name"]}! 😊\n\n" +
                       "I am here to help keep you safe online.\n" +
                       "What cybersecurity topic can I help you with today?\n\n" +
                       "Try asking about: passwords, phishing, scams, privacy, malware, 2FA, ransomware, or Wi-Fi!";
            }

            // Get the stored name for personalising responses
            string name = _memory.ContainsKey("name") ? _memory["name"] : "";
            string namePrefix = !string.IsNullOrEmpty(name) ? $"{name}, " : "";

            // Get a sentiment opener to add empathy to responses (Q6)
            string sentimentOpener = GetSentimentOpener(sentiment, namePrefix);

            
            // QUESTION 4 — CONVERSATION FLOW: Check for farewell words
            
            foreach (string word in _farewells)
            {
                if (lower.Contains(word))
                {
                    return $"👋 Goodbye{(!string.IsNullOrEmpty(name) ? ", " + name : "")}! " +
                           "Stay safe online. Remember — cybersecurity is everyone's responsibility! 🛡";
                }
            }

            
            // Check for greeting words
            
            foreach (string word in _greetings)
            {
                if (lower == word || lower.StartsWith(word + " ") || lower.EndsWith(" " + word))
                {
                    return $"👋 Hey{(!string.IsNullOrEmpty(name) ? ", " + name : "")}! " +
                           "Great to see you again! What cybersecurity topic can I help with today?";
                }
            }

            
            // QUESTION 4 — CONVERSATION FLOW: Follow-up on the same topic
            // e.g. user says "tell me more" or "give me another tip"
            
            foreach (string word in _followUps)
            {
                if (lower.Contains(word) && !string.IsNullOrEmpty(_lastTopic))
                {
                    string tip = GetRandomResponse(_lastTopic);
                    return $"{sentimentOpener}Here is another tip about {_lastTopic}:\n\n{tip}";
                }
            }

            
            // QUESTION 5 — MEMORY RECALL: User asks if bot remembers their name
            
            if (lower.Contains("my name") || lower.Contains("remember me") || lower.Contains("who am i"))
            {
                if (_memory.ContainsKey("name"))
                {
                    return $"🧠 Of course! You told me your name is {_memory["name"]}. I remember our conversation! 😊";
                }
                return "🤔 Hmm, I do not think you have told me your name yet! What should I call you?";
            }

            // QUESTION 5 — MEMORY: Store user's interest if they mention it
            
            if (lower.Contains("i like") || lower.Contains("i love") || lower.Contains("i am interested in"))
            {
                string topic = ExtractInterest(lower);
                if (!string.IsNullOrEmpty(topic))
                {
                    _memory["interest"] = topic;
                    return $"🧠 Got it{(!string.IsNullOrEmpty(name) ? ", " + name : "")}! " +
                           $"I will remember you are interested in {topic}. " +
                           "I can bring that up when it is relevant!";
                }
            }

            
            // QUESTION 2 — KEYWORD RECOGNITION
            // QUESTION 3 — RANDOM RESPONSES (picks randomly from the list)
            
            foreach (string keyword in _responses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    // Remember this topic for follow-up questions (Q4)
                    _lastTopic = keyword;

                    // Pick a random response from the list for this keyword (Q3)
                    string tip = GetRandomResponse(keyword);

                    // Add personalisation if we know their interest (Q5)
                    string extra = "";
                    if (_memory.ContainsKey("interest") && _memory["interest"].Contains(keyword))
                    {
                        extra = $"\n\n💡 Since you are interested in {keyword}, you may want to explore related topics too!";
                    }

                    return $"{sentimentOpener}{tip}{extra}\n\n" +
                           "💬 Want to know more? Just say 'tell me more' or ask about another topic!";
                }
            }

            
            // HELP COMMAND: Show list of available topics
            
            if (lower.Contains("help") || lower.Contains("menu") || lower.Contains("topics") || lower.Contains("what can you do"))
            {
                return $"🛡 {namePrefix}Here is what I can help you with:\n\n" +
                       "  🔐 Passwords\n" +
                       "  🎣 Phishing\n" +
                       "  🚨 Scams\n" +
                       "  👁 Privacy\n" +
                       "  🦠 Malware / Virus\n" +
                       "  📲 2FA\n" +
                       "  💰 Ransomware\n" +
                       "  📶 Wi-Fi\n" +
                       "  🎭 Social Engineering\n\n" +
                       "Just type any topic and I will share a cybersecurity tip!\n" +
                       "Say 'tell me more' for additional tips on the same topic.";
            }

            
            // QUESTION 7 — ERROR HANDLING: Unknown or unrecognised input
            // Picks a random fallback message so it does not feel repetitive
            
            return $"{namePrefix}{_unknownResponses[_random.Next(_unknownResponses.Count)]}";
        }

        
        // QUESTION 3 — RANDOM RESPONSE SELECTOR
        // Picks a random tip from the list for the matched keyword
        
        private string GetRandomResponse(string keyword)
        {
            if (_responses.ContainsKey(keyword))
            {
                List<string> options = _responses[keyword];
                return options[_random.Next(options.Count)];
            }
            return "I have some information on that topic. Could you be more specific?";
        }

        
        // QUESTION 6 — SENTIMENT OPENER
        // Adds an empathetic opening line based on the detected mood
        
        private string GetSentimentOpener(string sentiment, string namePrefix)
        {
            switch (sentiment)
            {
                case "worried":
                    return $"😟 I understand you are feeling worried, {namePrefix}and that is completely valid. Let me help reassure you:\n\n";
                case "frustrated":
                    return $"😤 I can hear your frustration, {namePrefix}and I am here to help make things clearer:\n\n";
                case "happy":
                    return $"😊 Great to hear you are feeling positive, {namePrefix}! Here is some useful info:\n\n";
                case "curious":
                    return $"🤔 Love the curiosity, {namePrefix}! Here is what you need to know:\n\n";
                default:
                    return "";
            }
        }

        
        // QUESTION 5 — MEMORY HELPER: Extract name from natural sentence
        // e.g. "my name is John" or "I am Sarah" → returns "John" or "Sarah"
        
        private string ExtractName(string input)
        {
            string lower = input.ToLower();
            string[] patterns = { "my name is ", "i am ", "i'm ", "call me ", "name's " };

            foreach (string pattern in patterns)
            {
                if (lower.Contains(pattern))
                {
                    int idx = lower.IndexOf(pattern) + pattern.Length;
                    string rest = input.Substring(idx).Trim().Split(' ')[0];
                    if (!string.IsNullOrEmpty(rest))
                    {
                        // Capitalise first letter
                        return char.ToUpper(rest[0]) + rest.Substring(1).ToLower();
                    }
                }
            }
            return "";
        }

        
        // QUESTION 5 — MEMORY HELPER: Find which cybersecurity topic interests them
        
        private string ExtractInterest(string lower)
        {
            foreach (string keyword in _responses.Keys)
            {
                if (lower.Contains(keyword)) return keyword;
            }
            return "";
        }

        
        // UI HELPER — Adds a USER message bubble to the chat
        
        private void AddUserMessage(string text)
        {
            // Outer bubble container (right-aligned, blue tint)
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
                CornerRadius = new CornerRadius(12, 12, 2, 12),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(80, 6, 10, 6),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 500
            };

            var stack = new StackPanel();

            // "You" label
            stack.Children.Add(new TextBlock
            {
                Text = "You",
                Foreground = new SolidColorBrush(Color.FromRgb(100, 180, 255)),
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 0, 0, 4)
            });

            // The actual message text
            stack.Children.Add(new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(230, 237, 243)),
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI"),
                TextWrapping = TextWrapping.Wrap
            });

            border.Child = stack;
            ChatPanel.Children.Add(border);
        }

        
        // UI HELPER — Adds a BOT message bubble to the chat
        
        private void AddBotMessage(string text)
        {
            // Outer bubble container (left-aligned, dark with green left border)
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(22, 27, 34)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 180, 100)),
                BorderThickness = new Thickness(1, 0, 0, 0),
                CornerRadius = new CornerRadius(2, 12, 12, 12),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(10, 6, 80, 6),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 560
            };

            var stack = new StackPanel();

            // "CyberGuard" label
            stack.Children.Add(new TextBlock
            {
                Text = "🛡 CyberGuard",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 136)),
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 0, 0, 4)
            });

            // The actual bot message text
            stack.Children.Add(new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(230, 237, 243)),
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI"),
                TextWrapping = TextWrapping.Wrap
            });

            border.Child = stack;
            ChatPanel.Children.Add(border);
        }

        
        // UI HELPER — Updates the memory counter in the status bar
        
        private void UpdateMemoryLabel()
        {
            int count = _memory.Count;
            MemoryLabel.Text = $"🧠 Memory: {count} item{(count == 1 ? "" : "s")}";
        }
    }
}
