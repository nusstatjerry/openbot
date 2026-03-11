


# 🚀千牛客服机器人

专为淘宝千牛打造的AI客服机器人，实现千牛7×24小时自动化值守，接入deepseek，阿里通义千问AI大模型



1. 仅支持千牛多账号模式
2. 需要打开千牛无障碍模式，如下图

<div align="center">
  <img src="https://worklink.oss-cn-hangzhou.aliyuncs.com/B78EE5CC4F478BE414D95C1CC12E20E2.png" >
  <br>
</div>

### 业务功能
1. 发图文消息 
2. 自动发货
3. 催付卡片 
4. 核对卡片 
5. 转接到个人 
6. 转接到分组 

## 🎨效果图

<div align="center">
  <img src="https://worklink.oss-cn-hangzhou.aliyuncs.com/9CD0A7CF011366063DB2E10A20462BA5.png" >
  <br>
</div>


### 环境要求
- .net framework4.8+


## 🛡 注意事项

⚠️ 注意：**本项目仅供学习与交流，如有侵权联系作者删除。**

鉴于项目的特殊性，开发团队可能在任何时间**停止更新**或**删除项目**。

## 交流群
欢迎加入项目交流群，交流技术、分享经验、互助学习。
<div align="center">
  <table>
    <tr>
      <td align="center"><strong>微信交流群</strong></td>
      <td align="center"><strong>微信赞赏码</strong></td>
    </tr>
    <tr>
      <td><img src="https://worklink.oss-cn-hangzhou.aliyuncs.com/CD0194ACF9A6794CBCFB035B5FE7DBF3.jpg"  width="300px" alt="微信交流群"></td>
      <td><img src="https://worklink.oss-cn-hangzhou.aliyuncs.com/D8512B5B322FFAAC3D6927982A12B078.jpg"  width="300px" alt="微信赞赏码"></td>
    </tr>
  </table>
</div>


### 打包便携运行包（不提交二进制到仓库）
如果你需要生成可分发压缩包，请在仓库根目录执行：

```bash
bash tools/package-portable.sh
```

默认会生成 `openbot-portable.zip`，包含：
- `src/Bin/`（`Bot.exe` 与依赖）
- `src/data/`（运行数据）
- `README.md`


### 大语言模型配置
在软件设置页（机器人选项）可以直接配置以下参数：
- `BaseUrl`：模型服务地址（可留空使用默认 OpenAI 地址）
- `ApiKey`：你的模型服务密钥
- `模型名称`：例如 `gpt-4o-mini`、`deepseek-chat`、`qwen-plus` 等
- `SystemPrompt`：系统提示词

保存后会按最新配置初始化 AI 客户端。


#### 豆包（火山方舟）示例
可在“机器人选项”里按下面填写：
- BaseUrl：`https://ark.cn-beijing.volces.com/api/v3`
- ApiKey：你的方舟 API Key（请勿提交到仓库）
- 模型名称：填写你在方舟创建的接入点/模型名（例如 `doubao-1-5-lite-32k-250115`）
- SystemPrompt：按你的业务场景填写

> 提示：如果你误填了 `.../chat/completions`，程序会自动归一化为服务根地址。
