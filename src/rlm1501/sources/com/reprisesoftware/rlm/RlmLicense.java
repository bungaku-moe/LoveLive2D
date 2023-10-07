package com.reprisesoftware.rlm;

/* loaded from: rlm1501.jar:com/reprisesoftware/rlm/RlmLicense.class */
public class RlmLicense implements RlmConstants {
    private long licHandle;
    private boolean valid;

    private native long rlmCheckout(long j, String str, String str2, int i);

    private native long rlmCheckoutProduct(long j, long j2, String str, int i);

    private native int rlmLicenseStat(long j);

    private native int rlmCheckin(long j);

    private native int rlmGetAttrHealth(long j);

    private native int rlmGoodOnce(long j);

    private native String rlmLicenseAkey(long j);

    private native String rlmLicenseContract(long j);

    private native String rlmLicenseCustomer(long j);

    private native int rlmLicenseCount(long j);

    private native int rlmLicenseDetachedDemo(long j);

    private native String rlmLicenseExp(long j);

    private native String rlmLicenseExpTime(long j);

    private native int rlmLicenseHold(long j);

    private native int rlmLicenseHostBased(long j);

    private native String rlmLicenseHostid(long j);

    private native int rlmLicenseIsMetered(long j);

    private native String rlmLicenseIssued(long j);

    private native String rlmLicenseIssuer(long j);

    private native String rlmLicenseLineItem(long j);

    private native int rlmLicenseMaxRoam(long j);

    private native int rlmLicenseMaxRoamCount(long j);

    private native int rlmLicenseMeterCounter(long j);

    private native int rlmLicenseMinRemove(long j);

    private native int rlmLicenseMinTimeout(long j);

    private native int rlmLicenseMinCheckout(long j);

    private native String rlmLicensePlatforms(long j);

    private native String rlmLicenseProduct(long j);

    private native String rlmLicenseServer(long j);

    private native int rlmLicenseSingle(long j);

    private native int rlmLicenseShare(long j);

    private native int rlmLicenseSoftLimit(long j);

    private native String rlmLicenseStart(long j);

    private native String rlmLicenseTeams(long j);

    private native int rlmLicenseType(long j);

    private native int rlmLicenseTimezone(long j);

    private native int rlmLicenseUncounted(long j);

    private native int rlmLicenseUserBased(long j);

    private native String rlmLicenseVersion(long j);

    private native int rlmLicenseNamedUserCount(long j);

    private native int rlmLicenseNamedUserMinHours(long j);

    private native String rlmLicenseGetLFPath(long j);

    private native int rlmLicenseExpDays(long j);

    private native int rlmLicenseRoaming(long j);

    private native int rlmLicenseMaxShare(long j);

    private native String rlmLicenseOptions(long j);

    private native int rlmLicenseClientCache(long j);

    private native int rlmLicenseIsCached(long j);

    private native int rlmAuthCheck(long j, String str);

    public RlmLicense(RlmHandle handle, String product, String version, int count) throws RlmException {
        this.licHandle = rlmCheckout(handle.getHandle(), product, version, count);
        int stat = rlmLicenseStat(this.licHandle);
        if (stat != 0 && stat != -25 && stat != -39) {
            throw new RlmException(stat, handle, this);
        }
        this.valid = true;
    }

    public RlmLicense(RlmHandle handle, RlmAvailableProduct product, String version, int count) throws RlmException {
        long prodHandle = product.getProdHandle();
        handle.positionProdHandle(prodHandle, product.getIndex());
        this.licHandle = rlmCheckoutProduct(handle.getHandle(), prodHandle, version, count);
        int stat = rlmLicenseStat(this.licHandle);
        if (stat != 0 && stat != -25 && stat != -39) {
            throw new RlmException(stat, handle, this);
        }
        this.valid = true;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public long getLicenseHandle() {
        return this.licHandle;
    }

    public int status() throws RlmException {
        if (this.valid) {
            int stat = rlmLicenseStat(this.licHandle);
            return stat;
        }
        throw new RlmException(-19);
    }

    public void checkin() {
        if (this.valid) {
            rlmCheckin(this.licHandle);
            this.valid = false;
        }
    }

    public int getAttrHealth() throws RlmException {
        if (this.valid) {
            int stat = rlmGetAttrHealth(this.licHandle);
            return stat;
        }
        throw new RlmException(-19);
    }

    public String getActKey() {
        return rlmLicenseAkey(this.licHandle);
    }

    public boolean goodOnce() {
        boolean ret = false;
        int stat = rlmGoodOnce(this.licHandle);
        if (stat != 0) {
            ret = true;
        }
        return ret;
    }

    public int getCLientCache() {
        return rlmLicenseClientCache(this.licHandle);
    }

    public String getContract() {
        return rlmLicenseContract(this.licHandle);
    }

    public int getDetachedDemo() {
        return rlmLicenseDetachedDemo(this.licHandle);
    }

    public String getCustomer() {
        return rlmLicenseCustomer(this.licHandle);
    }

    public int getCount() {
        return rlmLicenseCount(this.licHandle);
    }

    public String getExpiration() {
        return rlmLicenseExp(this.licHandle);
    }

    public String getExpTime() {
        return rlmLicenseExpTime(this.licHandle);
    }

    public int getHold() {
        return rlmLicenseHold(this.licHandle);
    }

    public int hostBased() {
        return rlmLicenseHostBased(this.licHandle);
    }

    public String getHostID() {
        return rlmLicenseHostid(this.licHandle);
    }

    public String getIssued() {
        return rlmLicenseIssued(this.licHandle);
    }

    public boolean isMetered() {
        int metered = rlmLicenseIsMetered(this.licHandle);
        return metered != 0;
    }

    public String getIssuer() {
        return rlmLicenseIssuer(this.licHandle);
    }

    public boolean isCached() {
        int cached = rlmLicenseIsCached(this.licHandle);
        return cached != 0;
    }

    public String getLineItem() {
        return rlmLicenseLineItem(this.licHandle);
    }

    public int getMaxRoam() {
        return rlmLicenseMaxRoam(this.licHandle);
    }

    public int getMaxRoamCount() {
        return rlmLicenseMaxRoamCount(this.licHandle);
    }

    public int getMeterCounter() {
        return rlmLicenseMeterCounter(this.licHandle);
    }

    public int getMinRemove() {
        return rlmLicenseMinRemove(this.licHandle);
    }

    public int getMinTimeout() {
        return rlmLicenseMinTimeout(this.licHandle);
    }

    public String getPlatforms() {
        return rlmLicensePlatforms(this.licHandle);
    }

    public String getProduct() {
        return rlmLicenseProduct(this.licHandle);
    }

    public String getServer() {
        return rlmLicenseServer(this.licHandle);
    }

    public int getShare() {
        return rlmLicenseShare(this.licHandle);
    }

    public int getSoftLimit() {
        return rlmLicenseSoftLimit(this.licHandle);
    }

    public String getStart() {
        return rlmLicenseStart(this.licHandle);
    }

    public int getType() {
        return rlmLicenseType(this.licHandle);
    }

    public String getTeamsURL() {
        return rlmLicenseTeams(this.licHandle);
    }

    public int getTimezone() {
        return rlmLicenseTimezone(this.licHandle);
    }

    public int userBased() {
        return rlmLicenseUserBased(this.licHandle);
    }

    public String getVersion() {
        return rlmLicenseVersion(this.licHandle);
    }

    public String getLFPath() {
        return rlmLicenseGetLFPath(this.licHandle);
    }

    public int daysToExpiration() {
        return rlmLicenseExpDays(this.licHandle);
    }

    public boolean isRoaming() {
        boolean roaming = false;
        if (rlmLicenseRoaming(this.licHandle) != 0) {
            roaming = true;
        }
        return roaming;
    }

    public int getMaxShare() {
        return rlmLicenseMaxShare(this.licHandle);
    }

    public int getNamedUserCount() {
        return rlmLicenseNamedUserCount(this.licHandle);
    }

    public int getNamedUserMinHours() {
        return rlmLicenseNamedUserMinHours(this.licHandle);
    }

    public String getOptions() {
        return rlmLicenseOptions(this.licHandle);
    }

    public int getMinCheckout() {
        return rlmLicenseMinCheckout(this.licHandle);
    }

    public boolean authCheck(String license) throws RlmException {
        int stat = rlmAuthCheck(this.licHandle, license);
        if (stat != 0) {
            throw new RlmException(stat);
        }
        return true;
    }

    public boolean isSingle() {
        boolean single = false;
        if (rlmLicenseSingle(this.licHandle) != 0) {
            single = true;
        }
        return single;
    }

    public boolean isUncounted() {
        boolean uncounted = false;
        if (rlmLicenseUncounted(this.licHandle) != 0) {
            uncounted = true;
        }
        return uncounted;
    }
}
