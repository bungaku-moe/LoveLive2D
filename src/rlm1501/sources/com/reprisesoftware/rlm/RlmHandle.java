package com.reprisesoftware.rlm;

import java.util.Vector;

/* loaded from: rlm1501.jar:com/reprisesoftware/rlm/RlmHandle.class */
public class RlmHandle implements RlmConstants {
    private long handle;
    private boolean previouslyActivated;
    private static final String defaultLibName = "rlm1501";
    private Vector productHandleList = null;

    private static native void rlmPutenv(String str);

    private native long rlmInit(String str, String str2, String str3);

    private native long rlmInitDisconn(String str, String str2, String str3, int i);

    private native int rlmClose(long j);

    private native int rlmStat(long j);

    private native int rlmHostID(long j, int i, byte[] bArr);

    private native void rlmAllHostIDs(long j, int i, byte[] bArr);

    private native int rlmRlog(long j, String str);

    private native int rlmDlog(long j, String str);

    private native int rlmDetachedDemo(long j, int i, String str);

    private native int rlmDetachedDemoX(long j, String str, String str2);

    private native void rlmDiagnostics(long j, String str);

    private native long rlmProducts(long j, String str, String str2);

    private native void rlmProductFirst(long j);

    private native int rlmProductNext(long j);

    private native void rlmProductFree(long j);

    private native String rlmProductName(long j);

    private native String rlmProductVersion(long j);

    private native String rlmProductActKey(long j);

    private native int rlmProductCount(long j);

    private native String rlmProductContract(long j);

    private native String rlmProductCustomer(long j);

    private native int rlmProductCurrentInUse(long j);

    private native int rlmProductCurrentResUse(long j);

    private native int rlmProductExpDays(long j);

    private native String rlmProductExpTime(long j);

    private native String rlmProductExpiration(long j);

    private native int rlmProductHostBased(long j);

    private native String rlmProductHostId(long j);

    private native int rlmProductHold(long j);

    private native int rlmProductIsAlias(long j);

    private native int rlmProductIsFloating(long j);

    private native int rlmProductIsNodelocked(long j);

    private native int rlmProductIsRoaming(long j);

    private native int rlmProductIsMetered(long j);

    private native int rlmProductIsSingle(long j);

    private native String rlmProductIssuer(long j);

    private native int rlmMaxRoam(long j);

    private native int rlmMaxRoamCount(long j);

    private native int rlmProductMeterCounter(long j);

    private native int rlmMeterCurCount(long j);

    private native int rlmMaxShare(long j);

    private native int rlmMinRemove(long j);

    private native int rlmMinCheckout(long j);

    private native int rlmMinTimeout(long j);

    private native int rlmProductNamedUserCount(long j);

    private native int rlmNRes(long j);

    private native int rlmNRoamAllowed(long j);

    private native String rlmOptions(long j);

    private native int rlmCurrentRoam(long j);

    private native String rlmServer(long j);

    private native int rlmShare(long j);

    private native int rlmSoftLimit(long j);

    private native String rlmProductStart(long j);

    private native int rlmTimeout(long j);

    private native int rlmTimezone(long j);

    private native int rlmIsTokenBased(long j);

    private native int rlmType(long j);

    private native int rlmUserBased(long j);

    private native int rlmClientCache(long j);

    private native void rlmSetUser(long j, String str);

    private native void rlmSetHost(long j, String str);

    private native void rlmSetIsvData(long j, String str);

    private native void rlmSkipIsvDown(long j);

    private native void rlmForgetIsvDown(long j);

    private native void rlmKeepConn(long j);

    private native void rlmErrstringNum(int i, byte[] bArr);

    private native void rlmEnableLogging(long j, int i);

    private native void rlmSetPassword(long j, String str);

    private native void rlmSetRequiredOption(long j, String str);

    private native int rlmActRequest(long j, String str, String str2, String str3, String str4, String str5, int i, String str6, byte[] bArr);

    private native int rlmActivate(long j, String str, String str2, int i, byte[] bArr, long j2);

    private native int rlmActRevoke(long j, String str, String str2);

    private native int rlmActRevokeReference(long j, String str, String str2);

    private native int rlmActRevokeDisconn(long j, String str, String str2, byte[] bArr);

    private native int rlmActRevokeRehost(long j, String str, String str2, int i);

    private native int rlmActKeyvalid(long j, String str, String str2, String str3);

    private native int rlmActKeyvalidLicense(long j, String str, String str2, String str3, byte[] bArr);

    private native int rlmGetRehost(long j, String str, byte[] bArr);

    private native void rlmSetReferenceHostid(long j, String str);

    public void loadLib(String libName) {
        System.loadLibrary(libName);
    }

    private void createHandle(String licLoc, String appPath, String license, String libName, String[] env, int promise) throws RlmException {
        loadLib(libName);
        if (env != null) {
            for (String str : env) {
                rlmPutenv(str);
            }
        }
        if (licLoc == null) {
            licLoc = "";
        }
        if (appPath == null) {
            appPath = "";
        }
        if (license == null) {
            license = "";
        }
        if (promise > 0) {
            this.handle = rlmInitDisconn(licLoc, appPath, license, promise);
        } else {
            this.handle = rlmInit(licLoc, appPath, license);
        }
        int stat = rlmStat(this.handle);
        if (stat != 0 && stat != -102) {
            throw new RlmException(stat, this);
        }
    }

    public RlmHandle(String licLoc, String appPath, String license) throws RlmException {
        createHandle(licLoc, appPath, license, defaultLibName, null, 0);
    }

    public RlmHandle(String licLoc, String appPath, String license, int promise) throws RlmException {
        createHandle(licLoc, appPath, license, defaultLibName, null, promise);
    }

    public RlmHandle(String licLoc, String appPath, String license, String[] env) throws RlmException {
        createHandle(licLoc, appPath, license, defaultLibName, env, 0);
    }

    public RlmHandle(String licLoc, String appPath, String license, String libName) throws RlmException {
        String lib;
        if (libName != null && libName.length() > 0) {
            lib = libName;
        } else {
            lib = defaultLibName;
        }
        createHandle(licLoc, appPath, license, lib, null, 0);
    }

    public RlmHandle(String licLoc, String appPath, String license, String libName, String[] env) throws RlmException {
        String lib;
        if (libName != null && libName.length() > 0) {
            lib = libName;
        } else {
            lib = defaultLibName;
        }
        createHandle(licLoc, appPath, license, lib, env, 0);
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public long getHandle() {
        return this.handle;
    }

    public void close() {
        if (this.handle != 0) {
            rlmClose(this.handle);
        }
        this.handle = 0L;
    }

    public static void putenv(String nvp) {
        rlmPutenv(nvp);
    }

    public String getHostID(int type) {
        byte[] buf = new byte[76];
        String hid = null;
        int stat = rlmHostID(this.handle, type, buf);
        if (stat == 0) {
            hid = new String(buf);
        }
        return hid;
    }

    public String[] getAllHostIDs(int type) {
        byte[] buf = new byte[1900];
        rlmAllHostIDs(this.handle, type, buf);
        String all = new String(buf).trim();
        String[] split = all.split(" ");
        return split;
    }

    public void writeReportLog(String logString) throws RlmException {
        int stat = rlmRlog(this.handle, logString);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public void writeDebugLog(String logString) throws RlmException {
        int stat = rlmDlog(this.handle, logString);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public void createDetachedDemo(int days, String license) throws RlmException {
        int stat = rlmDetachedDemo(this.handle, days, license);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public void removeDetachedDemo(String product, String version) throws RlmException {
        int stat = rlmDetachedDemoX(this.handle, product, version);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    private void addProduct(long prodHandle, Vector<RlmAvailableProduct> v, int index) {
        System.out.println("[Love Live2D] Adding fake product...");
        RlmAvailableProduct prod = new RlmAvailableProduct();
        prod.setName("live2d_editorc-3");
        prod.setVersion("3.0");
        prod.setActKey("CHCC-1234-5678-9012-3456");
        prod.setCount(0);
        prod.setCurrentInUse(0);
        prod.setCurrentResUse(0);
        prod.setContract("LiveDDDDDDDDD");
        prod.setCustomer("kiraio");
        prod.setExpDays(0);
        // prod.setExpTime("");
        prod.setExpiration("permanent");
        prod.setHostBased(0);
        prod.setHostId("ANY");
        prod.setHold(0);
        prod.setIsAlias(0);
        prod.setIsFloating(0);
        prod.setIsMetered(0);
        prod.setIsNodelocked(0);
        prod.setIsRoaming(0);
        prod.setIsSingle(0);
        prod.setIssuer("kiraio");
        prod.setMaxRoam(0);
        prod.setMaxRoamCount(0);
        prod.setMeterCounter(0);
        prod.setMeterCurCount(0);
        prod.setMaxShare(99);
        prod.setMinRemove(0);
        prod.setMinCheckout(0);
        prod.setMinTimeout(0);
        prod.setNamedUserCount(0);
        prod.setNRes(0);
        prod.setNRoamAllowed(0);
        prod.setOptions("");
        prod.setCurrentRoam(0);
        prod.setServer("127.0.0.1");
        prod.setShare(0);
        prod.setSoftLimit(0);
        prod.setStart("");
        prod.setTimeout(0);
        prod.setTimezone(0);
        prod.setIsTokenBased(0);
        prod.setType(0);
        prod.setUserBased(0);
        prod.setClientCache(0);
        prod.setProdHandle(prodHandle);
        prod.setIndex(index);
        v.add(prod);

        /*
        * ORIGINAL CODE
        */
        // RlmAvailableProduct prod = new RlmAvailableProduct();
        // prod.setName(rlmProductName(prodHandle));
        // prod.setVersion(rlmProductVersion(prodHandle));
        // prod.setActKey(rlmProductActKey(prodHandle));
        // prod.setCount(rlmProductCount(prodHandle));
        // prod.setCurrentInUse(rlmProductCurrentInUse(prodHandle));
        // prod.setCurrentResUse(rlmProductCurrentResUse(prodHandle));
        // prod.setContract(rlmProductContract(prodHandle));
        // prod.setCustomer(rlmProductCustomer(prodHandle));
        // prod.setExpDays(rlmProductExpDays(prodHandle));
        // prod.setExpTime(rlmProductExpTime(prodHandle));
        // prod.setExpiration(rlmProductExpiration(prodHandle));
        // prod.setHostBased(rlmProductHostBased(prodHandle));
        // prod.setHostId(rlmProductHostId(prodHandle));
        // prod.setHold(rlmProductHold(prodHandle));
        // prod.setIsAlias(rlmProductIsAlias(prodHandle));
        // prod.setIsFloating(rlmProductIsFloating(prodHandle));
        // prod.setIsMetered(rlmProductIsMetered(prodHandle));
        // prod.setIsNodelocked(rlmProductIsNodelocked(prodHandle));
        // prod.setIsRoaming(rlmProductIsRoaming(prodHandle));
        // prod.setIsSingle(rlmProductIsSingle(prodHandle));
        // prod.setIssuer(rlmProductIssuer(prodHandle));
        // prod.setMaxRoam(rlmMaxRoam(prodHandle));
        // prod.setMaxRoamCount(rlmMaxRoamCount(prodHandle));
        // prod.setMeterCounter(rlmProductMeterCounter(prodHandle));
        // prod.setMeterCurCount(rlmMeterCurCount(prodHandle));
        // prod.setMaxShare(rlmMaxShare(prodHandle));
        // prod.setMinRemove(rlmMinRemove(prodHandle));
        // prod.setMinCheckout(rlmMinCheckout(prodHandle));
        // prod.setMinTimeout(rlmMinTimeout(prodHandle));
        // prod.setNamedUserCount(rlmProductNamedUserCount(prodHandle));
        // prod.setNRes(rlmNRes(prodHandle));
        // prod.setNRoamAllowed(rlmNRoamAllowed(prodHandle));
        // prod.setOptions(rlmOptions(prodHandle));
        // prod.setCurrentRoam(rlmCurrentRoam(prodHandle));
        // prod.setServer(rlmServer(prodHandle));
        // prod.setShare(rlmShare(prodHandle));
        // prod.setSoftLimit(rlmSoftLimit(prodHandle));
        // prod.setStart(rlmProductStart(prodHandle));
        // prod.setTimeout(rlmTimeout(prodHandle));
        // prod.setTimezone(rlmTimezone(prodHandle));
        // prod.setIsTokenBased(rlmIsTokenBased(prodHandle));
        // prod.setType(rlmType(prodHandle));
        // prod.setUserBased(rlmUserBased(prodHandle));
        // prod.setClientCache(rlmClientCache(prodHandle));
        // prod.setProdHandle(prodHandle);
        // prod.setIndex(index);
        // v.add(prod);
    }

    public Vector getAvailableProducts(String product, String version) {
        System.out.println("[Love Live2D] Return fake product...");
        Vector<RlmAvailableProduct> v = new Vector<RlmAvailableProduct>();
        addProduct(1L, v, 1);
        return v;

        /*
        * ORIGINAL CODE
        */
        // Vector v = null;
        // long prodHandle = rlmProducts(this.handle, product, version);
        // if (prodHandle != 0) {
        //     v = new Vector();
        //     rlmProductFirst(prodHandle);
        //     int i = 0 + 1;
        //     addProduct(prodHandle, v, 0);
        //     while (rlmProductNext(prodHandle) == 0) {
        //         int i2 = i;
        //         i++;
        //         addProduct(prodHandle, v, i2);
        //     }
        //     if (this.productHandleList == null) {
        //         this.productHandleList = new Vector();
        //     }
        //     Long l = new Long(prodHandle);
        //     this.productHandleList.add(l);
        // }
        // return v;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void positionProdHandle(long prodHandle, int index) {
        rlmProductFirst(prodHandle);
        for (int i = 0; i < index; i++) {
            rlmProductNext(prodHandle);
        }
    }

    public void setUser(String user) {
        rlmSetUser(this.handle, user);
    }

    public void setHost(String host) {
        rlmSetHost(this.handle, host);
    }

    public void setIsvData(String isvData) {
        rlmSetIsvData(this.handle, isvData);
    }

    public void enableLogging(int enable) {
        rlmEnableLogging(this.handle, enable);
    }

    public void setPassword(String password) {
        rlmSetPassword(this.handle, password);
    }

    public void setOption(String option) {
        rlmSetRequiredOption(this.handle, option);
    }

    public void skipIsvDown() {
        rlmSkipIsvDown(this.handle);
    }

    public void forgetIsvDown() {
        rlmForgetIsvDown(this.handle);
    }

    public void keepConn() {
        rlmKeepConn(this.handle);
    }

    public String actRequest(String url, String isv, String key, String hostidList, String hostName, int count, String extra) throws RlmException {
        byte[] buf = new byte[102400];
        if (isv == null) {
            isv = "";
        }
        if (hostidList == null) {
            hostidList = "";
        }
        if (hostName == null) {
            hostName = "";
        }
        if (extra == null) {
            extra = "";
        }
        int stat = rlmActRequest(this.handle, url, isv, key, hostidList, hostName, count, extra, buf);
        if (stat < 0) {
            throw new RlmException(stat, this);
        }
        this.previouslyActivated = stat == 1;
        return new String(buf).trim();
    }

    public String activateLicense(String url, String key, int count, RlmActHandle actHandle) throws RlmException {
        String licenseActive = "[Love Live2D] Activate license...";
        System.out.println(licenseActive);
        return licenseActive;

        /*
        * ORIGINAL CODE
        */
        // long ah;
        // byte[] buf = new byte[102400];
        // if (actHandle == null) {
        //     ah = 0;
        // } else {
        //     ah = actHandle.getHandle();
        // }
        // int stat = rlmActivate(this.handle, url, key, count, buf, ah);
        // if (stat < 0) {
        //     throw new RlmException(stat, this);
        // }
        // this.previouslyActivated = stat == 1;
        // return new String(buf).trim();
    }

    public void revokeRehostable(String url, String product, int flags) throws RlmException {
        int stat = rlmActRevokeRehost(this.handle, url, product, flags);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public void revokeLicense(String url, String product) throws RlmException {
        int stat = rlmActRevoke(this.handle, url, product);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public void revokeLicenseReference(String url, String product) throws RlmException {
        int stat = rlmActRevokeReference(this.handle, url, product);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public String disconnectedRevoke(String product) throws RlmException {
        byte[] retval = new byte[1025];
        int stat = rlmActRevokeDisconn(this.handle, "", product, retval);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
        return new String(retval).trim();
    }

    public void processDisconnectedRevoke(String url, String verifyCode) throws RlmException {
        int stat = rlmActRevokeDisconn(this.handle, url, verifyCode, null);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
    }

    public boolean actKeyValid(String url, String actKey, String hostID) throws RlmException {
        System.out.println("[Love Live2D] Bypass activation key validity...");
        return true;

        /*
        * ORIGINAL CODE
        */
        // int stat = rlmActKeyvalid(this.handle, url, actKey, hostID);
        // if (stat != 0) {
        //     throw new RlmException(stat, this);
        // }
        // return true;
    }

    public String actKeyValidLicense(String url, String actKey, String hostID) throws RlmException {
        byte[] buf = new byte[102400];
        int stat = rlmActKeyvalidLicense(this.handle, url, actKey, hostID, buf);
        if (stat != 0) {
            throw new RlmException(stat, this);
        }
        return new String(buf).trim();
    }

    public boolean previouslyActivated() {
        return this.previouslyActivated;
    }

    public String getRehost(String product) {
        String ret = null;
        byte[] hostid = new byte[77];
        int stat = rlmGetRehost(this.handle, product, hostid);
        if (stat == 0) {
            ret = new String(hostid).trim();
        }
        return ret;
    }

    public void setReferenceHostid(String hostid) {
        rlmSetReferenceHostid(this.handle, hostid);
    }

    public void doClientSideDiagnostics(String diagFile) {
        rlmDiagnostics(this.handle, diagFile);
    }

    protected void finalize() {
        int n = this.productHandleList.size();
        for (int i = 0; i < n; i++) {
            Long l = (Long) this.productHandleList.get(i);
            long prodHandle = l.longValue();
            rlmProductFree(prodHandle);
        }
        close();
    }

    public String getMessageByCode(int code) {
        byte[] errString = new byte[513];
        rlmErrstringNum(code, errString);
        String ret = new String(errString).trim();
        return ret;
    }
}
