using UnityEngine;

/// <summary>Generic class which interfaces with <see cref="PlayerPrefs"/> to store and retrieve a value.</summary>
/// <remarks>
/// <para>Only handles <c>int</c>, <c>float</c>, <c>string</c>, and <c>bool</c> types.</para>
/// <para>Observers can subscribe to the <see cref="Setting{T}.SettingChangeHandler"> to be notified when the setting value is changed.</para>
/// <para>However, obervers must be careful to unsubscribe to avoid memory leak issues.</para>
///</remarks>
/// <seealso cref="FloatSetting"/>
/// <seealso cref="IntSetting"/>
/// <seealso cref="StringSetting"/>
/// <seealso cref="BoolSetting"/>
public abstract class Setting<T>
{   
    /// <summary>The default value this setting should take on if no corresponding value is stored in <see cref="PlayerPrefs"/>.</summary>
    protected T _defaultValue;

    /// <summary>The name of this setting, which is used as the key for retrieving values from <see cref="PlayerPrefs"/>.</summary>
    protected string _name;
    public string name => _name;

    /// <summary>The event which is invoked when the value of the setting is changed.</summary>
    public event System.Action<T> onChange;

    /// <summary>This property retrieves and stores values to and from <see cref="PlayerPrefs"/>.</summary>
    /// <value>The value of this setting.</value>
    public abstract T Value {get; set;}

    /// <summary>Constructor which initializes protected member variables.</summary>
    /// <param name="name">The <see cref="Setting{T}._name">name</see> of this <see cref="Setting{T}"/>.</param>
    /// <typeparam name="defaultValue">The <see cref="Setting{T}._defaultValue">default value</see> of this <see cref="Setting{T}"/>.</typeparam>
    public Setting(string name, T defaultValue)
    {
        _defaultValue = defaultValue;
        _name = name;
    }

    /// <summary>Helper function allowing subclasses to invoke <see cref="Setting{T}.onChange"/>.</summary>
    protected void InvokeChange(T newValue) => onChange?.Invoke(newValue);
}

/// <inheritdoc/>
/// <summary><see cref="Setting{T}"/> subclass which interfaces with <see cref="PlayerPrefs"/> to store and retrieve<c>float</c> values.</summary>
/// <remarks>
/// <para>Observers can subscribe to the <see cref="Setting{T}.SettingChangeHandler"> to be notified when the setting value is changed.</para>
/// <para>However, obervers must be careful to unsubscribe to avoid memory leak issues.</para>
///</remarks>
internal class FloatSetting : Setting<float>
{
    /// <summary>Constructor which simply calls the <see cref="Setting{T}.Setting(string, T)">base constructor</see>.</summary>
    public FloatSetting(string name, float defaultValue) : base(name, defaultValue) {}
    public override float Value
    {
        get => PlayerPrefs.GetFloat(_name, _defaultValue);
        set
        {
            PlayerPrefs.SetFloat(_name, value);
            InvokeChange(value);
        }
    }
}

/// <inheritdoc/>
/// <summary><see cref="Setting{T}"/> subclass which interfaces with <see cref="PlayerPrefs"/> to store and retrieve<c>int</c> values.</summary>
/// <remarks>
/// <para>Observers can subscribe to the <see cref="Setting{T}.SettingChangeHandler"> to be notified when the setting value is changed.</para>
/// <para>However, obervers must be careful to unsubscribe to avoid memory leak issues.</para>
///</remarks>
internal class IntSetting : Setting<int>
{
    /// <summary>Constructor which simply calls the <see cref="Setting{T}.Setting(string, T)">base constructor</see>.</summary>
    public IntSetting(string name, int defaultValue) : base(name, defaultValue) {}
    public override int Value
    {
        get => PlayerPrefs.GetInt(_name, _defaultValue);
        set
        {
            PlayerPrefs.SetInt(_name, value);
            InvokeChange(value);
        }
    }
}

/// <inheritdoc/>
/// <summary><see cref="Setting{T}"/> subclass which interfaces with <see cref="PlayerPrefs"/> to store and retrieve<c>string</c> values.</summary>
/// <remarks>
/// <para>Observers can subscribe to the <see cref="Setting{T}.SettingChangeHandler"> to be notified when the setting value is changed.</para>
/// <para>However, obervers must be careful to unsubscribe to avoid memory leak issues.</para>
///</remarks>
internal class StringSetting : Setting<string>
{
    /// <summary>Constructor which simply calls the <see cref="Setting{T}.Setting(string, T)">base constructor</see>.</summary>
    public StringSetting(string name, string defaultValue) : base(name, defaultValue) {}
    public override string Value
    {
        get => PlayerPrefs.GetString(_name, _defaultValue);
        set
        {
            PlayerPrefs.SetString(_name, value);
            InvokeChange(value);
        }
    }
}

/// <inheritdoc/>
/// <summary><see cref="Setting{T}"/> subclass which interfaces with <see cref="PlayerPrefs"/> to store and retrieve<c>bool</c> values.</summary>
/// <remarks>
/// <para>Observers can subscribe to the <see cref="Setting{T}.SettingChangeHandler"> to be notified when the setting value is changed.</para>
/// <para>However, obervers must be careful to unsubscribe to avoid memory leak issues.</para>
///</remarks>
internal class BoolSetting : Setting<bool>
{
    /// <summary>Constructor which simply calls the <see cref="Setting{T}.Setting(string, T)">base constructor</see>.</summary>
    public BoolSetting(string name, bool defaultValue) : base(name, defaultValue) {}
    public override bool Value
    {
        get => PlayerPrefs.GetInt(_name, _defaultValue ? 1 : 0) > 0 ? true : false;
        set
        {
            PlayerPrefs.SetInt(_name, value ? 1 : 0);
            InvokeChange(value);
        }
    }
}