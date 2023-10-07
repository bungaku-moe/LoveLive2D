package com.reprisesoftware.rlm;

/* loaded from: rlm1501.jar:com/reprisesoftware/rlm/RlmAvailableProduct.class */
public class RlmAvailableProduct implements RlmConstants {
    private String name;
    private String version;
    private String actKey;
    private int count;
    private String contract;
    private String customer;
    private int expDays;
    private String expTime;
    private String expiration;
    private int inUse;
    private int resInUse;
    private int hostBased;
    private String hostId;
    private int hold;
    private boolean isMetered;
    private String issuer;
    private int maxRoam;
    private int maxRoamCount;
    private int maxShare;
    private int meterCounter;
    private int meterCurCount;
    private int minRemove;
    private int minCheckout;
    private int minTimeout;
    private int namedUserCount;
    private int nRes;
    private int nRoamAllowed;
    private String options;
    private int currentRoam;
    private String server;
    private int share;
    private int softLimit;
    private String start;
    private int timeout;
    private int timezone;
    private boolean isAlias;
    private boolean isFloating;
    private boolean isNodelocked;
    private boolean isRoaming;
    private boolean isSingle;
    private boolean isTokenBased;
    private int type;
    private int userBased;
    private int clientCache;
    private long prodHandle;
    private int index;

    /* JADX INFO: Access modifiers changed from: protected */
    public void setName(String name) {
        this.name = name;
    }

    public String getName() {
        return this.name;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setVersion(String version) {
        this.version = version;
    }

    public String getVersion() {
        return this.version;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setActKey(String actKey) {
        this.actKey = actKey;
    }

    public String getActKey() {
        return this.actKey;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setCount(int count) {
        this.count = count;
    }

    public int getCount() {
        return this.count;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setContract(String contract) {
        this.contract = contract;
    }

    public String getContract() {
        return this.contract;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setCustomer(String customer) {
        this.customer = customer;
    }

    public String getCustomer() {
        return this.customer;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setExpTime(String expTime) {
        this.expTime = expTime;
    }

    public String getExpTime() {
        return this.expTime;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setExpiration(String expiration) {
        this.expiration = expiration;
    }

    public String getExpiration() {
        return this.expiration;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setExpDays(int expDays) {
        this.expDays = expDays;
    }

    public int getExpDays() {
        return this.expDays;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setCurrentInUse(int count) {
        this.inUse = count;
    }

    public int getCurrentInUse() {
        return this.inUse;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setCurrentResUse(int count) {
        this.resInUse = count;
    }

    public int getCurrentResUse() {
        return this.resInUse;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setHostBased(int count) {
        this.hostBased = count;
    }

    public int getHostBased() {
        return this.hostBased;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setHostId(String hostId) {
        this.hostId = hostId;
    }

    public String getHostId() {
        return this.hostId;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setHold(int count) {
        this.hold = count;
    }

    public int getHold() {
        return this.hold;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsMetered(int isMetered) {
        if (isMetered != 0) {
            this.isMetered = true;
        } else {
            this.isMetered = false;
        }
    }

    public boolean getIsMetered() {
        return this.isMetered;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIssuer(String issuer) {
        this.issuer = issuer;
    }

    public String getIssuer() {
        return this.issuer;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMaxRoam(int count) {
        this.maxRoam = count;
    }

    public int getMaxRoam() {
        return this.maxRoam;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMaxRoamCount(int count) {
        this.maxRoamCount = count;
    }

    public int getMaxRoamCount() {
        return this.maxRoamCount;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMaxShare(int count) {
        this.maxShare = count;
    }

    public int getMaxShare() {
        return this.maxShare;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMeterCounter(int counter) {
        this.meterCounter = counter;
    }

    public int getMeterCounter() {
        return this.meterCounter;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMeterCurCount(int count) {
        this.meterCurCount = count;
    }

    public int getMeterCurCount() {
        return this.meterCurCount;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMinCheckout(int count) {
        this.minCheckout = count;
    }

    public int getMinCheckout() {
        return this.minCheckout;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMinTimeout(int count) {
        this.minTimeout = count;
    }

    public int getMinTimeout() {
        return this.minTimeout;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setMinRemove(int count) {
        this.minRemove = count;
    }

    public int getMinRemove() {
        return this.minRemove;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setNamedUserCount(int count) {
        this.namedUserCount = count;
    }

    public int getNamedUserCount() {
        return this.namedUserCount;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setNRes(int count) {
        this.nRes = count;
    }

    public int getNRes() {
        return this.nRes;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setNRoamAllowed(int count) {
        this.nRoamAllowed = count;
    }

    public int getNRoamAllowed() {
        return this.nRoamAllowed;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setCurrentRoam(int count) {
        this.currentRoam = count;
    }

    public int getCurrentRoam() {
        return this.currentRoam;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setOptions(String options) {
        this.options = options;
    }

    public String getOptions() {
        return this.options;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setServer(String server) {
        this.server = server;
    }

    public String getServer() {
        return this.server;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setShare(int count) {
        this.share = count;
    }

    public int getShare() {
        return this.share;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setSoftLimit(int count) {
        this.softLimit = count;
    }

    public int getSoftLimit() {
        return this.softLimit;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setStart(String start) {
        this.start = start;
    }

    public String getStart() {
        return this.start;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setTimeout(int count) {
        this.timeout = count;
    }

    public int getTimeout() {
        return this.timeout;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setTimezone(int count) {
        this.timezone = count;
    }

    public int getTimezone() {
        return this.timezone;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsAlias(int alias) {
        if (alias == 0) {
            this.isAlias = false;
        } else {
            this.isAlias = true;
        }
    }

    public boolean getIsAlias() {
        return this.isAlias;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsFloating(int floating) {
        if (floating == 0) {
            this.isFloating = false;
        } else {
            this.isFloating = true;
        }
    }

    public boolean getIsFloating() {
        return this.isFloating;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsNodelocked(int nodelocked) {
        if (nodelocked == 0) {
            this.isNodelocked = false;
        } else {
            this.isNodelocked = true;
        }
    }

    public boolean getIsNodelocked() {
        return this.isNodelocked;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsRoaming(int roaming) {
        if (roaming == 0) {
            this.isRoaming = false;
        } else {
            this.isRoaming = true;
        }
    }

    public boolean getIsRoaming() {
        return this.isRoaming;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsSingle(int single) {
        if (single == 0) {
            this.isSingle = false;
        } else {
            this.isSingle = true;
        }
    }

    public boolean getIsSingle() {
        return this.isSingle;
    }

    protected void setIsTokenBased(boolean tokenBased) {
        this.isTokenBased = tokenBased;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIsTokenBased(int tokenBased) {
        if (tokenBased == 0) {
            this.isTokenBased = false;
        } else {
            this.isTokenBased = true;
        }
    }

    public boolean getIsTokenBased() {
        return this.isTokenBased;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setType(int count) {
        this.type = count;
    }

    public int getType() {
        return this.type;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setUserBased(int count) {
        this.userBased = count;
    }

    public int getUserBased() {
        return this.userBased;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setClientCache(int cache) {
        this.clientCache = cache;
    }

    protected int getClientCache() {
        return this.clientCache;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setProdHandle(long prodHandle) {
        this.prodHandle = prodHandle;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public long getProdHandle() {
        return this.prodHandle;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public void setIndex(int index) {
        this.index = index;
    }

    /* JADX INFO: Access modifiers changed from: protected */
    public int getIndex() {
        return this.index;
    }
}
