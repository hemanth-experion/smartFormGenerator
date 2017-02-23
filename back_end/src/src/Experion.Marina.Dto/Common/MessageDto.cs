using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Experion.Marina.Dto
{
    /// <summary>
    /// Different message statuses
    /// </summary>
    public enum MessageStatus
    {
        Success = 0,
        Warning = 1,
        Question = 2,
        Error = 3
    }

    /// <summary>
    /// Different message types
    /// </summary>
    public enum MessageType
    {
        Error,
        Question,
        Warning,
        Information
    }

    /// <summary>
    /// The Message class which specify the type of message and the message text
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageKey">The message key.</param>
        public Message(MessageType messageType, string messageKey) : this(messageType, messageKey, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageKey">The message key.</param>
        /// <param name="messageParam">The message parameter.</param>
        public Message(MessageType messageType, string messageKey, string messageParam) : this(messageType, messageKey, new string[] { messageParam })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageKey">The message key.</param>
        /// <param name="messageParams">The message parameters.</param>
        public Message(MessageType messageType, string messageKey, string[] messageParams)
        {
            MessageType = messageType;
            MessageKey = messageKey;
            MessageParams = messageParams;
        }

        public string MessageKey { get; set; }
        [JsonIgnore]
        public string[] MessageParams { get; set; }

        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
    }

    public class MessageDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDto"/> class.
        /// </summary>
        public MessageDto()
        {
        }

        /// <summary>
        /// Gets a value indicating whether this instance has messages.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has messages; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool HasMessages
        {
            get
            {
                if (Messages != null && Messages.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public List<Message> Messages { get; set; } = new List<Message>();

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public MessageStatus Status
        {
            get
            {
                if (Messages == null)
                {
                    return MessageStatus.Success;
                }
                else
                {
                    return GetMessageStatus();
                }
            }
        }
        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public void AddError(string errorMessage)
        {
            Messages.Add(new Message(MessageType.Error, errorMessage));
        }

        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="informationMessage">The information message.</param>
        public void AddInformation(string informationMessage)
        {
            Messages.Add(new Message(MessageType.Information, informationMessage));
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageKey">The message key.</param>
        public void AddMessage(MessageType messageType, string messageKey)
        {
            AddMessage(messageType, messageKey, string.Empty);
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageKey">The message key.</param>
        /// <param name="messageParam">The message parameter.</param>
        public void AddMessage(MessageType messageType, string messageKey, string messageParam)
        {
            Messages.Add(new Message(messageType, messageKey, messageParam));
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageKey">The message key.</param>
        /// <param name="messageParams">The message parameters.</param>
        public void AddMessage(MessageType messageType, string messageKey, string[] messageParams)
        {
            Messages.Add(new Message(messageType, messageKey, messageParams));
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// Adds the question.
        /// </summary>
        /// <param name="questionMessage">The question message.</param>
        public void AddQuestion(string questionMessage)
        {
            Messages.Add(new Message(MessageType.Question, questionMessage));
        }

        /// <summary>
        /// Adds the warning.
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        public void AddWarning(string warningMessage)
        {
            Messages.Add(new Message(MessageType.Warning, warningMessage));
        }

        /// <summary>
        /// Determines whether the specified message key contains message.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        /// <returns></returns>
        public bool ContainsMessage(string messageKey)
        {
            if (Messages != null && Messages.Any())
            {
                return Messages.FindIndex(key => key.MessageKey == messageKey) >= 0;
            }
            return false;
        }

        /// <summary>
        /// Removes the message.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        public void RemoveMessage(string messageKey)
        {
            if (Messages != null && Messages.Any())
            {
                var messageIndex = Messages.FindIndex(key => key.MessageKey == messageKey);
                if (messageIndex >= 0)
                {
                    Messages.RemoveAt(messageIndex);
                }
            }
        }

        /// <summary>
        /// Gets the response status.
        /// </summary>
        /// <returns></returns>
        private MessageStatus GetMessageStatus()
        {
            if (Messages != null && Messages.Any())
            {
                if (Messages.Any(m => m.MessageType == MessageType.Error))
                {
                    return MessageStatus.Error;
                }
                else if (Messages.Any(m => m.MessageType == MessageType.Question))
                {
                    return MessageStatus.Question;
                }
                else if (Messages.Any(m => m.MessageType == MessageType.Warning))
                {
                    return MessageStatus.Warning;
                }
            }
            return MessageStatus.Success;
        }
    }
}