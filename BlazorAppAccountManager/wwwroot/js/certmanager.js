function handleFileSelect() {
    alert("hello file");
}

let dotNetHelperInstance = null;

// 1. 新增一个初始化函数，用于接收 Blazor 传递过来的实例引用
function initFileHandler(dotNetHelper) {
    dotNetHelperInstance = dotNetHelper;
};
function loadPdffile(inputElement) {
    if (!inputElement || !inputElement.files || inputElement.files.length === 0) {
        return;
    }

    const file = inputElement.files[0];

    // 1. 创建 IJSStreamReference
    const streamRef = DotNet.createJSStreamReference(file);

    const url = URL.createObjectURL(file);

    // 2. 构造 DTO 对象
    const fileData = {
        name: file.name,
        size: file.size,
        contentType: file.type,
        streamReference: streamRef,
        Url: url
    };

    // 3. 使用传递过来的 C# 对象引用来调用 C# 实例方法
    // 'ReceiveNativeFile' 必须与 C# 中 [JSInvokable] 方法名一致
    dotNetHelperInstance.invokeMethodAsync('ReceiveNativeFile', fileData);
};

// 从input获取文件信息并创建Blob URL
function getFileInfoAndBlobUrl (inputElement) {
    if (!inputElement.files || inputElement.files.length === 0) {
        return null;
    }

    const file = inputElement.files[0];
    // 创建Blob URL（不会复制文件内容，仅创建引用）
    const blobUrl = URL.createObjectURL(file);

    return {
        name: file.name,
        size: file.size,
        type: file.type,
        blobUrl: blobUrl
    };
};


/**
* 清除 <input type="file"> 元素的值，使其显示为空。
* @param {HTMLInputElement} inputElement - 通过 @ref 传递过来的 input 元素。
*/
function clearFileInput(inputElement) {
    if (inputElement && inputElement.type === 'file') {
        // 将 value 属性设为 null 或空字符串可以清除文件选择
        inputElement.value = null;

        // 有些浏览器可能需要显式设置为空字符串
        if (!inputElement.value) {
            inputElement.value = '';
        }
    }
};