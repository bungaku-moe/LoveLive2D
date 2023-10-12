package com.reprisesoftware.rlm;

/* loaded from: rlm1501.jar:com/reprisesoftware/rlm/RlmTempHandle.class */
public class RlmTempHandle implements RlmConstants {
    private static int RLM_TEMP_HANDLE_AKEY = 1;
    private static int RLM_TEMP_HANDLE_COUNT = 2;
    private static int RLM_TEMP_HANDLE_DURATION = 3;
    private static int RLM_TEMP_HANDLE_LICENSE = 4;
    private static int RLM_TEMP_HANDLE_NEW_DURATION = 5;
    private static int RLM_TEMP_HANDLE_SOURCE = 6;
    private static int RLM_TEMP_HANDLE_TYPE = 7;
    private static int RLM_TEMP_HANDLE_URL = 8;
    private static int RLM_TEMP_HANDLE_WINDOW = 9;
    private long tempHandleL;
    private long rlmHandleL;
    private RlmHandle rlmHandle;

    private static native long rlmTempNewHandle(long j);

    private static native void rlmTempDestroyHandle(long j);

    private static native int rlmTempSetHandle(long j, int i, String str);

    private static native int rlmTempSetHandleInt(long j, int i, int i2);

    private static native int rlmTempSetHandleLong(long j, int i, long j2);

    private static native int rlmTempCreateLicense(long j, long j2);

    private static native int rlmTempReturnLicense(long j);

    public RlmTempHandle(RlmHandle rh) {
        this.rlmHandle = rh;
        this.rlmHandleL = rh.getHandle();
        this.tempHandleL = rlmTempNewHandle(this.rlmHandleL);
    }

    public void create() throws RlmException {
        int stat = rlmTempCreateLicense(this.rlmHandleL, this.tempHandleL);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void returnLicense(RlmLicense tempLicense) throws RlmException {
        int stat = rlmTempReturnLicense(tempLicense.getLicenseHandle());
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setNewDuration(int minutes) throws RlmException {
        int stat = rlmTempSetHandleInt(this.tempHandleL, RLM_TEMP_HANDLE_NEW_DURATION, minutes);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setDuration(int minutes) throws RlmException {
        int stat = rlmTempSetHandleInt(this.tempHandleL, RLM_TEMP_HANDLE_DURATION, minutes);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setLicense(RlmLicense license) throws RlmException {
        int stat = rlmTempSetHandleLong(this.tempHandleL, RLM_TEMP_HANDLE_LICENSE, license.getLicenseHandle());
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setLicenseType(int type) throws RlmException {
        int stat = rlmTempSetHandleInt(this.tempHandleL, RLM_TEMP_HANDLE_TYPE, type);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setWindow(int minutes) throws RlmException {
        int stat = rlmTempSetHandleInt(this.tempHandleL, RLM_TEMP_HANDLE_WINDOW, minutes);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    protected long getHandle() {
        return this.tempHandleL;
    }

    protected void finalize() {
        rlmTempDestroyHandle(this.tempHandleL);
    }
}
