using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Words : MonoBehaviour {
	public static event Action <string> OnWordSetChar;
	public static event Action <string> OnUpdateText;
	
	[SerializeField] private Text _text;
	
	private readonly string[] _words = {"HELLO", "WORLD", "BEST", "LUCK", "PEACE", "FRIEND", "SURPRISE", "DINOSAUR"};// 0 - Available, 1 - Active, 2 - Used
	private readonly int[] _wordsAvailable = new int[8];
	private string _originalWord = "";
	private String _currentWord = "";
	private int _wordId;
	private int _charId;

	private const string SecretSign = "●";
	private const char SecretSignChar = '●';

	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < _words.Length; i++)
		{
			int state = PlayerPrefs.GetInt ("Words_Available" + i, 0);
			_wordsAvailable [i] = state;
			if (state == 1)
			{
				_wordId = i;
				_originalWord = _words [i];
				_currentWord = PlayerPrefs.GetString("Words_CurrentWord", "");
			}
		}

		if (WordsOverCheck())
		{
			GlobalEvents<OnWordsAvailable>.Call(new OnWordsAvailable{IsAvailable = false});
		}
		else
		{
			GlobalEvents<OnWordsAvailable>.Call(new OnWordsAvailable{IsAvailable = true});
			if (_originalWord == "")
			{
				_wordId = 0;
				SetWordById();
			}

			SetNextChar();
		}
	}

	private void OnEnable()
	{
		BonusChar.OnBonusGot += OnBonusGot;
		GlobalEvents<OnBtnWordClick>.Happened += OnBtnWordClick;
	}

	private void OnBtnWordClick(OnBtnWordClick obj)
	{
		if (WordsOverCheck())
		{
			GlobalEvents<OnWordsAvailable>.Call(new OnWordsAvailable{IsAvailable = false});
		}
		else
		{
			_wordId = GetNextWordId();
			SetWordById();
			SetNextChar();
		}
	}

	//--------------------------------------------------------------
	//							CHARS
	//--------------------------------------------------------------

	private void SetNextChar() {
		_charId = GetNextCharId();
		_text.text = _currentWord;
		GameEvents.Send(OnWordSetChar,_originalWord.Substring(_charId,1));
		
		D.Log("WORDS: we have a new CHAR = ", _originalWord[_charId]);
	}

	private void OnBonusGot()
	{
		ReplaceCharInString(ref _currentWord, _charId, _originalWord[_charId]);
		if (CheckCurrWord())
		{
			GlobalEvents<OnWordCollected>.Call(new OnWordCollected {Text = _originalWord});
			GlobalEvents<OnWordUpdateProgress>.Call(new OnWordUpdateProgress {Text = _currentWord});
			SaveCurrentWord();
		}
		else
		{
			GlobalEvents<OnWordUpdateProgress>.Call(new OnWordUpdateProgress {Text = _currentWord});
			PlayerPrefs.SetString("Words_CurrentWord", _currentWord);
			SetNextChar();
		}

		GameEvents.Send(OnUpdateText, _currentWord);
	}

	private int GetNextCharId() {
		int id = Random.Range (0, _originalWord.Length-1);
		if (_currentWord [id] != SecretSignChar)
		{
			int i = 0;
			while (i < _currentWord.Length)	 {
				++id;
				if (id > _currentWord.Length - 1)
					id = 0;
				if (_currentWord [id] == SecretSignChar) {
					return id;
				}
				++i;
			}
		}
		return id;
	}

	//--------------------------------------------------------------
	//							WORDS
	//--------------------------------------------------------------

	private bool WordsOverCheck()
	{
		for (int i = 0; i < _wordsAvailable.Length; i++)
		{
			if (_wordsAvailable[i] != 2) return false;
		}
		return true;
	}

	private void SetWordById ()
	{
		_originalWord = _words [_wordId];
		_wordsAvailable [_wordId] = 1;
		PlayerPrefs.GetInt ("Words_Available" + _wordId, 1);
		PrepareNewWord (_originalWord);
		GlobalEvents<OnWordUpdateProgress>.Call(new OnWordUpdateProgress {Text = _currentWord});
		D.Log("WORDS: we have a new WORD = ", _originalWord);
	}
	
	private void PrepareNewWord (string originalWord)
	{
		_currentWord = "";
		for(int i = 0; i < originalWord.Length; i++)
		{
			_currentWord += SecretSign;
		}
	}

	private void SaveCurrentWord ()
	{
		_wordsAvailable [_wordId] = 2;
		PlayerPrefs.GetInt ("Words_Available" + _wordId, 2);
	}

	private bool CheckCurrWord ()
	{
		for (int i = 0; i < _currentWord.Length; i++) {
			if (_currentWord [i] == SecretSignChar) {
				return false;
			}
		}
		return true;
	}

	private int GetNextWordId() {
		int id = Random.Range (0, _wordsAvailable.Length-1);
		if (_wordsAvailable[id] == 0)
		{
			return id;
			
		}

		int i = 0;
		while (i < _wordsAvailable.Length)
		{
			++id;
			if (id > _wordsAvailable.Length - 1)
				id = 0;
			if (_wordsAvailable[id] == 0)
			{
				return id;
			}
		}

		return -1;
	}
	
	public void ReplaceCharInString(ref String str, int index, Char newSymb)
	{
		str = str.Remove(index, 1).Insert(index, newSymb.ToString());
	}
 
// либо:
//	public String ReplaceCharInString(String str, int index, Char newSymb)
//	{
//		return str.Remove(index, 1).Insert(index, newSymb.ToString());
//	}
}
