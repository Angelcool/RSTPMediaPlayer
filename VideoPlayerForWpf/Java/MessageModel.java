package cn.vsx.ptt.sdk.server.message;

import java.util.List;
import java.util.Map;

/**
 * @Author: liushengjie
 * @Date: 2019/4/20 22:25
 */
public class MessageModel {

    private String videoMessageType;

    private String memberNo;

    /**
     * 提示信息
     */
    private String prompt;

    private String uniqueNo;

    private VideoPlayModel videoData;

    private List<VideoPlayModel> videoDataList;

    private boolean collectStat;

    private String data;

    private String memberMessageType;

    private WatchSubordinateLiveMessageBean watchSubordinateLiveMessageBean;

    private Map<String,Object> videoMap ;

    public String getVideoMessageType() {
        return videoMessageType;
    }

    public void setVideoMessageType(String videoMessageType) {
        this.videoMessageType = videoMessageType;
    }

    public String getMemberNo() {
        return memberNo;
    }

    public void setMemberNo(String memberNo) {
        this.memberNo = memberNo;
    }

    public String getPrompt() {
        return prompt;
    }

    public void setPrompt(String prompt) {
        this.prompt = prompt;
    }

    public String getUniqueNo() {
        return uniqueNo;
    }

    public void setUniqueNo(String uniqueNo) {
        this.uniqueNo = uniqueNo;
    }

    public VideoPlayModel getVideoData() {
        return videoData;
    }

    public void setVideoData(VideoPlayModel videoData) {
        this.videoData = videoData;
    }

    public boolean isCollectStat() {
        return collectStat;
    }

    public void setCollectStat(boolean collectStat) {
        this.collectStat = collectStat;
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public String getMemberMessageType() {
        return memberMessageType;
    }

    public void setMemberMessageType(String memberMessageType) {
        this.memberMessageType = memberMessageType;
    }

    public WatchSubordinateLiveMessageBean getWatchSubordinateLiveMessageBean() {
        return watchSubordinateLiveMessageBean;
    }

    public void setWatchSubordinateLiveMessageBean(WatchSubordinateLiveMessageBean watchSubordinateLiveMessageBean) {
        this.watchSubordinateLiveMessageBean = watchSubordinateLiveMessageBean;
    }

    public List<VideoPlayModel> getVideoDataList() {
        return videoDataList;
    }

    public void setVideoDataList(List<VideoPlayModel> videoDataList) {
        this.videoDataList = videoDataList;
    }

    public Map<String, Object> getVideoMap() {
        return videoMap;
    }

    public void setVideoMap(Map<String, Object> videoMap) {
        this.videoMap = videoMap;
    }
}
