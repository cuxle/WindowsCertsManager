// 打开文件选择器并返回文件信息
export function openFilePicker() {
    return new Promise((resolve) => {
        // 创建一个隐藏的file input元素
        const fileInput = document.createElement('input');
        fileInput.type = 'file';
        fileInput.style.display = 'none';

        // 添加到文档中
        document.body.appendChild(fileInput);

        // 监听文件选择事件
        fileInput.addEventListener('change', (e) => {
            const file = e.target.files[0];
            if (file) {
                // 创建一个临时URL用于预览
                const fileUrl = URL.createObjectURL(file);

                // 返回文件信息
                resolve({
                    name: file.name,
                    type: file.type,
                    size: file.size,
                    url: fileUrl
                });
            } else {
                resolve(null);
            }

            // 清理
            document.body.removeChild(fileInput);
        });

        // 触发文件选择对话框
        fileInput.click();
    });
}
