package cn.vsx.ptt.sdk.server.message;

/**
 * @Author: liushengjie
 * @Date: 2019/4/20 22:27
 */
public class VideoPlayModel {

    private int groupNo;

    private String groupName;

    /**
     * 是否显示加载动画
     */
    private boolean showLoading;

    private String isCamera;

    private String liveMemberName;

    private String liveMemberNo;

    private String liveMemberUniqueNo;

    private String resultCode;

    private String resultDesc;

    private String rtspUrl;

    private String theme;

    private String callId;

    private String terminalMemberType;

    private String streamType;

    /**
     * osd叠层信息，没有可以不传
     */
    private OSD osd;


    /**
     * 视频点名 排序
     */
    private Integer sort;

    /**
     * 是否铺满
     */
    private Boolean covered;

    /**
     * 是否显示云台控制
     */
    private boolean showPtz;
    private String title;
    /**
     * 是否显示组呼按钮
     */
    private boolean showGroupCall;
    private boolean coverageFocus = true ;

    public int getGroupNo() {
        return groupNo;
    }

    public void setGroupNo(int groupNo) {
        this.groupNo = groupNo;
    }

    public String getGroupName() {
        return groupName;
    }

    public void setGroupName(String groupName) {
        this.groupName = groupName;
    }

    public String getIsCamera() {
        return isCamera;
    }

    public void setIsCamera(String isCamera) {
        this.isCamera = isCamera;
    }

    public String getLiveMemberName() {
        return liveMemberName;
    }

    public void setLiveMemberName(String liveMemberName) {
        this.liveMemberName = liveMemberName;
    }

    public String getLiveMemberNo() {
        return liveMemberNo;
    }

    public void setLiveMemberNo(String liveMemberNo) {
        this.liveMemberNo = liveMemberNo;
    }

    public String getResultCode() {
        return resultCode;
    }

    public void setResultCode(String resultCode) {
        this.resultCode = resultCode;
    }

    public String getResultDesc() {
        return resultDesc;
    }

    public void setResultDesc(String resultDesc) {
        this.resultDesc = resultDesc;
    }

    public String getRtspUrl() {
        return rtspUrl;
    }

    public void setRtspUrl(String rtspUrl) {
        this.rtspUrl = rtspUrl;
    }

    public String getTheme() {
        return theme;
    }

    public void setTheme(String theme) {
        this.theme = theme;
    }

    public String getCallId() {
        return callId;
    }

    public void setCallId(String callId) {
        this.callId = callId;
    }

    public String getTerminalMemberType() {
        return terminalMemberType;
    }

    public void setTerminalMemberType(String terminalMemberType) {
        this.terminalMemberType = terminalMemberType;
    }

    public String getStreamType() {
        return streamType;
    }

    public void setStreamType(String streamType) {
        this.streamType = streamType;
    }

    public String getLiveMemberUniqueNo() {
        return liveMemberUniqueNo;
    }

    public void setLiveMemberUniqueNo(String liveMemberUniqueNo) {
        this.liveMemberUniqueNo = liveMemberUniqueNo;
    }

    public Integer getSort() {
        return sort;
    }

    public void setSort(Integer sort) {
        this.sort = sort;
    }

    public boolean isShowPtz() {
        return showPtz;
    }

    public void setShowPtz(boolean showPtz) {
        this.showPtz = showPtz;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public boolean isCoverageFocus() {
        return coverageFocus;
    }

    public void setCoverageFocus(boolean coverageFocus) {
        this.coverageFocus = coverageFocus;
    }

    public boolean isShowLoading() {
        return showLoading;
    }

    public void setShowLoading(boolean showLoading) {
        this.showLoading = showLoading;
    }

    public OSD getOsd() {
        return osd;
    }

    public void setOsd(OSD osd) {
        this.osd = osd;
    }

    public boolean isShowGroupCall() {
        return showGroupCall;
    }

    public void setShowGroupCall(boolean showGroupCall) {
        this.showGroupCall = showGroupCall;
    }
    public Boolean getCovered() {
        return covered;
    }

    public void setCovered(Boolean covered) {
        this.covered = covered;
    }
}
