package com.reprisesoftware.rlm;

/* loaded from: rlm1501.jar:com/reprisesoftware/rlm/RlmException.class */
public class RlmException extends Exception {
    private int code;
    RlmHandle handle;
    RlmLicense license;
    String message;

    private native void rlmErrstring(long j, long j2, byte[] bArr);

    /* JADX INFO: Access modifiers changed from: protected */
    public RlmException(int code) {
        this.code = code;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public RlmException(int code, RlmHandle handle, RlmLicense license) {
        this.handle = handle;
        this.license = license;
        this.code = code;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public RlmException(int code, RlmHandle handle) {
        this.handle = handle;
        this.license = null;
        this.code = code;
    }

    public int getCode() {
        return this.code;
    }

    @Override // java.lang.Throwable
    public String getMessage() {
        String ret;
        if (this.handle == null && this.license == null) {
            ret = new String("RlmException " + this.code);
        } else {
            byte[] errString = new byte[513];
            long h = 0;
            long l = 0;
            if (this.handle != null) {
                h = this.handle.getHandle();
            }
            if (this.license != null) {
                l = this.license.getLicenseHandle();
            }
            rlmErrstring(h, l, errString);
            ret = new String(errString).trim();
        }
        return ret;
    }
}
