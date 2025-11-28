// 自动登录脚本 - 在登录页面加载时执行
(function() {
    // 检查URL中是否包含自动登录参数
    const urlParams = new URLSearchParams(window.location.search);
    const isAutoLogin = urlParams.get('auto') === '1';
    
    if (isAutoLogin) {
        // 从localStorage获取登录信息
        const username = localStorage.getItem('autoLoginUsername');
        const password = localStorage.getItem('autoLoginPassword');
        
        if (username && password) {
            // 等待DOM加载完成
            document.addEventListener('DOMContentLoaded', function() {
                setTimeout(() => {
                    // 查找用户名和密码输入框
                    const usernameInput = document.querySelector('input[name="Input.UserName"]');
                    const passwordInput = document.querySelector('input[name="Input.Password"]');
                    const submitButton = document.querySelector('button[type="submit"]');
                    
                    if (usernameInput && passwordInput && submitButton) {
                        // 填充表单
                        usernameInput.value = username;
                        passwordInput.value = password;
                        
                        // 触发输入事件以确保验证通过
                        usernameInput.dispatchEvent(new Event('input', { bubbles: true }));
                        passwordInput.dispatchEvent(new Event('input', { bubbles: true }));
                        
                        console.log('自动填充登录表单，准备提交...');
                        
                        // 提交表单
                        submitButton.click();
                    } else {
                        console.error('找不到登录表单元素');
                    }
                    
                    // 清除localStorage中的敏感信息
                    localStorage.removeItem('autoLoginUsername');
                    localStorage.removeItem('autoLoginPassword');
                }, 1000); // 短暂延迟以确保页面完全加载
            });
        }
    }
})();