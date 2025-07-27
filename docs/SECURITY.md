# Security Best Practices

## File-Based License Storage

### Production Deployment
- Store license files outside application directory
- Use proper file permissions (600 for license, 644 for public key)
- Never commit actual license files to source control

### Container Deployment
- Use secrets management (Docker secrets, Kubernetes secrets)
- Mount license files as read-only volumes
- Set appropriate file ownership and permissions

For complete security guidance, see [Integration Guide](INTEGRATION.md).
