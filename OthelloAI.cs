using System;

public class OthelloAI {
    private String name;


    public OthelloAI(String name) {
	this.name = name;
    }

    public OthelloAI() {
	this.name = "名称未設定";
    }    
    
    public void setName(String name) {
	this.name = name;
    }
	
    public String getName() {
	return this.name;
    }
}
