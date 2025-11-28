using BlazorAppAccountManager.Components.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorAppAccountManager.Components.Services
{
    public class CertificateReferenceService
    {
        // 单个引用上下文（不再按用户ID区分）
        private readonly CertificateReferenceContext _context = new();
        private readonly object _lock = new object();

        // 注入身份验证服务（如果仍需验证用户登录状态）
        private readonly AuthenticationStateProvider _authStateProvider;

        public CertificateReferenceService(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        // 初始化引用
        public async Task InitReference(string sourceSupplierId, string sourceUrl)
        {
            // 如需验证用户已登录，可保留此检查
            await EnsureUserLoggedIn();
            Console.WriteLine($"Init Context:{_context.ToString()}");
            lock (_lock)
            {
                _context.CurrentReferenceMode = ReferenceMode.None;
                _context.CurrentCertificateType = CertificateType.Other;
                _context.SourceUrl = sourceUrl;
                _context.IsCompleted = false;
                _context.SupplierId = string.Empty; // 重置之前的选择
            }
        }

        // 完成引用
        public async Task CompleteReference(string supplierId)
        {
            Console.WriteLine("CompleteReference Called!");
            await EnsureUserLoggedIn();

            lock (_lock)
            {
                _context.SupplierId = supplierId;
                _context.IsCompleted = true;
            }

            // 触发回调
            if (OnReferenceCompleted != null)
            {
                await OnReferenceCompleted.Invoke();
            }
        }

        // 取消引用
        public Task CancelReference()
        {
            //lock (_lock)
            //{
            //    // 重置上下文
            //    _context.SourceUrl = string.Empty;
            //    _context.SupplierId = string.Empty;
            //    _context.IsCompleted = false;
            //}
            return Task.CompletedTask;
        }

        // 获取当前上下文
        public Task<CertificateReferenceContext> GetCurrentContext()
        {
            lock (_lock)
            {
                Console.WriteLine($"GetCurrentContext Context:{_context.ToString()}");
                // 返回上下文的副本避免外部直接修改内部状态（可选）
                return Task.FromResult(new CertificateReferenceContext
                {
                    SourceUrl = _context.SourceUrl,
                    SupplierId = _context.SupplierId,
                    IsCompleted = _context.IsCompleted,
                    CurrentCertificateType = _context.CurrentCertificateType,
                    CurrentReferenceMode = _context.CurrentReferenceMode,
                    SourceLampId = _context.SourceLampId
                });
            }
        }

        // 保存上下文（如果需要更新上下文的其他属性）
        public Task SaveContext(CertificateReferenceContext updatedContext)
        {
            if (updatedContext == null) return Task.CompletedTask;
            Console.WriteLine($"SaveContext Context1:{updatedContext.ToString()}");
            lock (_lock)
            {
                _context.CurrentCertificateType = updatedContext.CurrentCertificateType;
                _context.CurrentReferenceMode = updatedContext.CurrentReferenceMode;
                _context.SourceLampId = updatedContext.SourceLampId;
                _context.SourceUrl = updatedContext.SourceUrl;
                // 根据需要同步其他属性
            }
            Console.WriteLine($"SaveContext Context2:{_context.ToString()}");
            return Task.CompletedTask;
        }

        // 辅助方法：确保用户已登录（如果需要）
        private async Task EnsureUserLoggedIn()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            if (!authState.User.Identity?.IsAuthenticated ?? true)
            {
                throw new InvalidOperationException("用户未登录");
            }
        }

        // 引用完成后的回调事件
        public event Func<Task>? OnReferenceCompleted;
    }
}
