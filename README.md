 (cd "$(git rev-parse --show-toplevel)" && git apply --3way <<'EOF' 
diff --git a/README.md b/README.md
index 568fe853ec2e258168a86b5a0f7f5bf21f590da7..fe4d49bd2987261afbfa9717e5bd2646179953e9 100644
--- a/README.md
+++ b/README.md
@@ -33,25 +33,81 @@
 
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
+
+
+### 打包便携运行包（不提交二进制到仓库）
+如果你需要生成可分发压缩包，请在仓库根目录执行：
+
+```bash
+bash tools/package-portable.sh
+```
+
+默认会生成 `openbot-portable.zip`，包含：
+- `src/Bin/`（`Bot.exe` 与依赖）
+- `src/data/`（运行数据）
+- `README.md`
+
+
+### 大语言模型配置
+在软件设置页（机器人选项）可以直接配置以下参数：
+- `BaseUrl`：模型服务地址（可留空使用默认 OpenAI 地址）
+- `ApiKey`：你的模型服务密钥
+- `模型名称`：例如 `gpt-4o-mini`、`deepseek-chat`、`qwen-plus` 等
+- `SystemPrompt`：系统提示词
+
+保存后会按最新配置初始化 AI 客户端。
+
+
+#### 豆包（火山方舟）示例
+可在“机器人选项”里按下面填写：
+- BaseUrl：`https://ark.cn-beijing.volces.com/api/v3`
+- ApiKey：你的方舟 API Key（请勿提交到仓库）
+- 模型名称：填写你在方舟创建的接入点/模型名（例如 `doubao-1-5-lite-32k-250115`）
+- SystemPrompt：按你的业务场景填写
+
+> 提示：如果你误填了 `.../chat/completions`，程序会自动归一化为服务根地址。
+
+
+### Windows 双击即启动
+如果你已经能在 `src/Bin/` 目录直接运行，那么确实不一定需要再次打包。
+
+你可以使用以下两种更方便的方式：
+- 直接双击：`src/Bin/启动灵桥客服.cmd`
+- 生成桌面图标（推荐）：在 PowerShell 执行
+
+```powershell
+powershell -ExecutionPolicy Bypass -File tools/create-windows-shortcut.ps1
+```
+
+执行后会在桌面生成“灵桥客服”快捷方式（图标来自 `Bot.exe`）。
+
+
+### 界面优化说明（最新）
+- 默认字体：`Microsoft YaHei UI`（Windows 兼容）
+- 文本框：聚焦时高亮边框，输入状态更清晰
+- 按钮：悬停 / 按下 / 禁用态更明显
+- 列表与分组框：统一浅色边框与留白，视觉更简洁
+
+以上优化不影响原有业务功能与启动方式。
 
EOF
)