package cn.vsx.ptt.sdk.server.enums;

import java.util.EnumSet;
import java.util.HashMap;
import java.util.Map;

/**
 * @author sean
 */

public enum VideoMessageType {
    START("播放视频",1,"start"),
    STOP("退出观看/停止播放",2,"stop"),
    OPENPUSH("推送",3,"openPush"),
    SELECT("选中窗口",4,"select"),
    PUSHMANY("多路视频推送",5,"pushMany"),
    PUSH2SPLIT("预览窗口推送到九分屏",6,"push2Split"),
    PTZCONTROL("云台控制",7,"ptzControl"),
    SHOW("显示窗体",8,"show"),
    PROMPT("显示提示",9,"prompt"),
    HEART("心跳",10,"heart"),
    ROLLCALLSTATUS("点名状态变更",11,"rollcallstatus");

    private String value;
    private int code;
    private String mark;
    private static Map<Integer, VideoMessageType> code2VideoMessageType = new HashMap<>();

    private VideoMessageType(String value, int code,String mark) {
        this.setValue(value);
        this.setCode(code);
        this.setMark(mark);
    }

    static {
        for (VideoMessageType businessStatus : EnumSet.allOf(VideoMessageType.class)) {
            // Yes, use some appropriate locale in production code :)
            code2VideoMessageType.put(businessStatus.getCode(), businessStatus);
        }
    }

    public static VideoMessageType getInstanceByCode(int code) {
        return code2VideoMessageType.get(code);
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getMark() {
        return mark;
    }

    public void setMark(String mark) {
        this.mark = mark;
    }

    public String toLowerCase(){
        return this.toString().toLowerCase();
    }
}
