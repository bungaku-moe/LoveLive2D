package com.reprisesoftware.rlm;

/* loaded from: rlm1501.jar:com/reprisesoftware/rlm/RlmActHandle.class */
public class RlmActHandle implements RlmConstants {
    private static int RLM_ACT_HANDLE_ISV = 1;
    private static int RLM_ACT_HANDLE_HOSTID_LIST = 2;
    private static int RLM_ACT_HANDLE_HOSTNAME = 3;
    private static int RLM_ACT_HANDLE_EXTRA = 4;
    private static int RLM_ACT_HANDLE_LOG = 5;
    private static int RLM_ACT_HANDLE_REHOST = 6;
    private static int RLM_ACT_HANDLE_DISCONN = 7;
    private static int RLM_ACT_HANDLE_PRODUCT = 8;
    private static int RLM_ACT_HANDLE_CONTACT = 10;
    private static int RLM_ACT_HANDLE_COMPANY = 11;
    private static int RLM_ACT_HANDLE_EMAIL = 12;
    private static int RLM_ACT_HANDLE_STATE = 13;
    private static int RLM_ACT_HANDLE_COUNTRY = 14;
    private static int RLM_ACT_HANDLE_U1 = 15;
    private static int RLM_ACT_HANDLE_U2 = 16;
    private long actHandleL;
    private long rlmHandleL;
    private RlmHandle rlmHandle;

    private static native long rlmActNewHandle(long j);

    private static native void rlmActDestroyHandle(long j);

    private static native int rlmActSetHandle(long j, int i, String str);

    private static native int rlmActSetHandleInt(long j, int i, int i2);

    public RlmActHandle(RlmHandle rh) {
        this.rlmHandle = rh;
        this.rlmHandleL = rh.getHandle();
        this.actHandleL = rlmActNewHandle(this.rlmHandleL);
    }

    public void setExtra(String extra) throws RlmException {
        int stat = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_EXTRA, extra);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setHostidList(String hostidList) throws RlmException {
        int stat = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_HOSTID_LIST, hostidList);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setHostname(String hostname) throws RlmException {
        int stat = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_HOSTNAME, hostname);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setLogString(String logString) throws RlmException {
        int stat = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_LOG, logString);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setISV(String ISV) throws RlmException {
        int stat = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_ISV, ISV);
        if (stat != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    public void setRehostable(boolean rehostable) {
        rlmActSetHandleInt(this.actHandleL, RLM_ACT_HANDLE_REHOST, rehostable ? 1 : 0);
    }

    public void setCustomerInfo(String contact, String company, String email, String state, String country, String u1, String u2) throws RlmException {
        int stat;
        int stat2;
        int stat3;
        int stat4;
        int stat5;
        int stat6 = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_CONTACT, contact);
        if (stat6 != 0) {
            throw new RlmException(stat6, this.rlmHandle);
        }
        int stat7 = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_COMPANY, company);
        if (stat7 != 0) {
            throw new RlmException(stat7, this.rlmHandle);
        }
        if (email != null && (stat5 = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_EMAIL, email)) != 0) {
            throw new RlmException(stat5, this.rlmHandle);
        }
        if (state != null && (stat4 = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_STATE, state)) != 0) {
            throw new RlmException(stat4, this.rlmHandle);
        }
        if (country != null && (stat3 = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_COUNTRY, country)) != 0) {
            throw new RlmException(stat3, this.rlmHandle);
        }
        if (u1 != null && (stat2 = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_U1, u1)) != 0) {
            throw new RlmException(stat2, this.rlmHandle);
        }
        if (u2 != null && (stat = rlmActSetHandle(this.actHandleL, RLM_ACT_HANDLE_U2, u2)) != 0) {
            throw new RlmException(stat, this.rlmHandle);
        }
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public long getHandle() {
        return this.actHandleL;
    }

    protected void finalize() {
        rlmActDestroyHandle(this.actHandleL);
    }
}
