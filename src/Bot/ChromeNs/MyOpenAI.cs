using BotLib.Extensions;
using Newtonsoft.Json.Linq;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Bot.ChromeNs
{
    public class MyOpenAI
    {
        public static ChatClient ChatClient { get; set; }

        private static string systemPrompt;
        private static string currentApiKey;
        private static string currentBaseUrl;
        private static string currentModel;
        private static string clientInitError;

        private static readonly object chatClientLock = new object();

        private static ConcurrentDictionary<string, List<ChatMessage>> buyerChatMessages;

        static MyOpenAI()
        {
            buyerChatMessages = new ConcurrentDictionary<string, List<ChatMessage>>();
            RefreshSettingsAndClient();
        }

        private static string NormalizeBaseUrl(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return string.Empty;
            }

            var normalized = baseUrl.Trim();
            normalized = normalized.Replace("/chat/completions", string.Empty).TrimEnd('/');
            return normalized;
        }

        private static void RefreshSettingsAndClient()
        {
            var apikey = Params.Robot.GetApiKey();
            var baseUrl = NormalizeBaseUrl(Params.Robot.GetBaseUrl());
            var model = Params.Robot.GetModelName();
            systemPrompt = Params.Robot.GetSystemPrompt();
            if (string.IsNullOrWhiteSpace(systemPrompt))
            {
                systemPrompt = "你是一个专业的电商客服助手，请基于用户问题给出简洁、礼貌且可执行的回复。";
            }

            lock (chatClientLock)
            {
                var isChanged = !string.Equals(currentApiKey, apikey, StringComparison.Ordinal)
                    || !string.Equals(currentBaseUrl, baseUrl, StringComparison.Ordinal)
                    || !string.Equals(currentModel, model, StringComparison.Ordinal);

                if (!isChanged)
                {
                    return;
                }

                currentApiKey = apikey;
                currentBaseUrl = baseUrl;
                currentModel = model;
                clientInitError = null;

                if (!string.IsNullOrEmpty(apikey) && !string.IsNullOrEmpty(model))
                {
                    if (string.IsNullOrEmpty(baseUrl))
                    {
                        ChatClient = new ChatClient(model: model, credential: new System.ClientModel.ApiKeyCredential(apikey));
                        return;
                    }

                    if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var endpoint))
                    {
                        ChatClient = null;
                        clientInitError = "错误：BaseUrl 不是有效地址，请检查配置";
                        return;
                    }

                    ChatClient = new ChatClient(model: model, credential: new System.ClientModel.ApiKeyCredential(apikey), options: new OpenAIClientOptions
                    {
                        Endpoint = endpoint
                    });
                }
                else
                {
                    ChatClient = null;
                }
            }
        }

        public static string GetAnswer(string seller, string buyer, string question)
        {
            RefreshSettingsAndClient();

            if (ChatClient == null)
            {
                if (!string.IsNullOrEmpty(clientInitError))
                {
                    return clientInitError;
                }

                if (string.IsNullOrEmpty(currentApiKey) || string.IsNullOrEmpty(currentModel))
                {
                    return "错误：未配置AI参数，请在设置中配置API密钥和模型名称";
                }

                return "错误：AI客户端未正确初始化";
            }

            var key = string.Format("{0}#{1}", seller, buyer);
            var messages = buyerChatMessages.xTryGetValue(key);
            if (messages == null || messages.Count < 1)
            {
                messages = new List<ChatMessage>() {
                    ChatMessage.CreateSystemMessage(systemPrompt),
                    ChatMessage.CreateUserMessage(question),
                };
            }
            else
            {
                messages.Add(ChatMessage.CreateUserMessage(question));
            }

            string answer;
            try
            {
                var completion = ChatClient.CompleteChat(messages);
                var completionContent = completion.GetRawResponse().Content.ToString();
                answer = JObject.Parse(completionContent)["choices"]?[0]?["message"]?["content"]?.ToString();
            }
            catch (Exception ex)
            {
                return "错误：调用AI服务失败（" + ex.Message + "）";
            }

            if (string.IsNullOrWhiteSpace(answer))
            {
                return "错误：AI服务返回了空结果，请检查模型配置或稍后重试";
            }

            messages.Add(ChatMessage.CreateAssistantMessage(answer));
            buyerChatMessages.AddOrUpdate(key, id => messages, (k, v) => messages);
            return answer;
        }
    }
}
