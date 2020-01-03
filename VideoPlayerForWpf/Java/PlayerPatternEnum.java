package cn.vsx.ptt.sdk.server.enums;

import java.util.EnumSet;
import java.util.HashMap;
import java.util.Map;

/**
 * @author sean
 */

public enum PlayerPatternEnum {
    PREVIEW("预览模式",1),
    SPLIT("分屏模式",2),
    CRUISE("巡航模式",3),
    CALL("点名模式",4),
    PREVIEW_VERTICAL("竖屏预览模式",5);

    private String value;
    private int code;
    private static Map<Integer, PlayerPatternEnum> code2PlayerPatternEnum = new HashMap<>();

    private PlayerPatternEnum(String value, int code) {
        this.setValue(value);
        this.setCode(code);
    }

    static {
        for (PlayerPatternEnum businessStatus : EnumSet.allOf(PlayerPatternEnum.class)) {
            // Yes, use some appropriate locale in production code :)
            code2PlayerPatternEnum.put(businessStatus.getCode(), businessStatus);
        }
    }

    public static PlayerPatternEnum getInstanceByCode(int code) {
        return code2PlayerPatternEnum.get(code);
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
}
