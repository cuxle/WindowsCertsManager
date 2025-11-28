namespace BlazorAppAccountManager.Components.Models
{
    public class CertificateReferenceContext
    {
        public string? SourceLampId { get; set; }
        public string? UserId { get; set; } // 可用于验证
        // 发起引用的源供应商ID（谁要引用）
        public string SourceUrl { get; set; } = string.Empty;

        private ReferenceMode _currentReferenceMode = ReferenceMode.Normal;
        public ReferenceMode CurrentReferenceMode
        {
            get => _currentReferenceMode;
            set
            {
                // 记录旧值和新值
                //Logger.LogInformation($"CurrentReferenceMode 变更: 旧值={_currentReferenceMode}, 新值={value}, 调用栈={Environment.StackTrace}");
                _currentReferenceMode = value;
            }
        }

        public CertificateType CurrentCertificateType { get; set; } = CertificateType.EMark;

        public string SupplierId { get; set; }

        // 引用完成后选中的目标证书ID
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"CurrentReferenceMode: {CurrentReferenceMode}, " +
                   $"CurrentCertificateType: {CurrentCertificateType}, " +
                   $"SourceUrl: {SourceUrl}, " +
                   $"IsCompleted: {IsCompleted}, " +
                   $"SupplierId: {SupplierId}, " +
                   $"SourceLampId: {SourceLampId}"; // 按需添加所有成员
        }
    }
}
